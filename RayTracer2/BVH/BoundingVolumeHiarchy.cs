using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RayTracer2.Geometry;

namespace RayTracer2.BVH
{
    public class BVHNode
    {
        public BVHNode[] Children;
        public int ShapeID = -1;
        public BoundingBox BBox;

        public BVHNode(BVHNode Child1, BVHNode Child2, BoundingBox Box)
        {
            Children = new BVHNode[2] { Child1, Child2 };

            BBox = Box;
        }

        public BVHNode(int ShapeID, BoundingBox Box)
        {
            this.ShapeID = ShapeID;

            BBox = Box;
        }
    }

    public struct BucketInfo
    {
        public int Count;
        public BoundingBox BBox;
    }

    public class BoundingVolumeHiarchy
    {
        private BVHNode RootNode;
        private IShape[] Shapes;

        public BoundingVolumeHiarchy(IShape[] Shapes)
        {
            this.Shapes = Shapes;
            if (Shapes.Length > 0)
            {
                RootNode = BuildRecurse(0, this.Shapes.Length - 1);
            }
            else
            {
                RootNode = new BVHNode(-1, new BoundingBox(new Point(0, 0, 0)));
            }
        }

        public BoundingBox getBoundingBox()
        {
            return RootNode.BBox;
        }

        public IShape getIntersectedShape(Ray r)
        {
            return getIntersectedShapeRecurse(r, RootNode);
        }

        public IShape getIntersectedShapeRecurse(Ray r, BVHNode n)
        {

            // get all intersections from children
            if (Geometry.Geometry.Intersect(r, n.BBox))
            {
                IShape[] intersected = new IShape[2];

                if (n.Children != null)
                {
                    Parallel.For(0, n.Children.Length, index =>
                    {
                        if (n.Children[index] != null)
                        {
                            intersected[index] = getIntersectedShapeRecurse(r, n.Children[index]);
                        }
                    });
                } else if (n.ShapeID != -1)
                {
                    intersected[0] = Shapes[n.ShapeID].getIntersectedShape(r);
                }


                // If there is more than one intersect, get the closest to the ray.
                if (intersected[0] != null && intersected[1] != null)
                {
                    double[] intersectTimes = new double[2];

                    // Get both intersect times
                    Parallel.For(0, intersected.Length, index =>
                    {
                        intersectTimes[index] = intersected[index].getIntersectTime(r);
                    });

                    // Return the intersected shape that's closest
                    if (intersectTimes[0] < intersectTimes[1])
                    {
                        return intersected[0];
                    }
                    else
                    {
                        return intersected[1];
                    }
                }
                else if (intersected[0] != null)
                {
                    return intersected[0];
                }
                else
                {
                    return intersected[1];
                }
            }

            return null;
        }

        private void QuickSortRecurse(int StarIndex, int EndIndex, Axis a)
        {
            IShape pivot = Shapes[(StarIndex + EndIndex) / 2];

            int i = StarIndex, j = EndIndex;

            while (i <= j)
            {
                while (Shapes[i].getBoundingBox().Centroid[a] > pivot.getBoundingBox().Centroid[a] && i <= j)
                {
                    i++;
                }

                while (Shapes[j].getBoundingBox().Centroid[a] < pivot.getBoundingBox().Centroid[a] && i <= j)
                {
                    j--;
                }

                if (i <= j)
                {
                    //Swap
                    IShape temp = Shapes[i];
                    Shapes[i] = Shapes[j];
                    Shapes[j] = temp;

                    i++;
                    j--;
                }

            }

            Parallel.Invoke(() =>
            {
                if (StarIndex < j)
                {
                    QuickSortRecurse(StarIndex, j, a);
                }

            },
            () =>
            {
                if (StarIndex < j && j <= EndIndex)
                {
                    QuickSortRecurse(j, EndIndex, a);
                }
            });

        }

        private BVHNode BuildRecurse(int StartNode, int EndNode, int SortedAxis = -1)
        {
            BVHNode node = null;
            BoundingBox centroidBounds = null;
            BoundingBox totalBounds = null;
            for (int i = StartNode; i <= EndNode; i++)
            {
                centroidBounds = Geometry.Geometry.Union(centroidBounds, Shapes[i].getBoundingBox().Centroid);
                totalBounds = Geometry.Geometry.Union(totalBounds, Shapes[i].getBoundingBox());
            }


            // Let's figure out what he axis that we want to do the split on
            Axis axis = centroidBounds.GetMaxextent();

            // Time to sort our list so that we can split it in half later
            if (StartNode <= EndNode)
            {
                if (SortedAxis != (int)axis)
                {
                    QuickSortRecurse(StartNode, EndNode, axis);
                }
            } else
            {
                throw new Exception("StartNode is greater than EndNode");
            }


            BVHNode[] Children = new BVHNode[2];
            if (EndNode - StartNode < 4)
            {

                Parallel.For(0, Children.Length, index =>
                {
                    int index0 = StartNode + index * 2;

                    BVHNode[] nodes = new BVHNode[2];

                    for (int i = 0; i < nodes.Length; i++)
                    {
                        if (index0 + i < Shapes.Length && index0 + i <= EndNode)
                        {
                            nodes[i] = new BVHNode(index0 + i, Shapes[index0 + i].getBoundingBox());
                        }
                    }

                    if (nodes[0] != null && nodes[1] != null)
                    {
                        Children[index] = new BVHNode(nodes[0], nodes[1], Geometry.Geometry.Union(nodes[0].BBox, nodes[1].BBox));

                    } else if (nodes[0] != null)
                    {
                        Children[index] = nodes[0];
                    } else
                    {
                        Children[index] = nodes[1];
                    }
                });

            } else
            {

                // Let's get our buckets and place each item in the correct bucket

                BucketInfo[] buckets = new BucketInfo[12]; 
                
                for (int i =StartNode; i <= EndNode; i++)
                {
                    int bucketInt = (int) (buckets.Length * centroidBounds.Offset(Shapes[i].getBoundingBox().Centroid)[axis]);
                    if (bucketInt == buckets.Length) bucketInt = buckets.Length - 1;
                    if (bucketInt > buckets.Length)
                    {
                        throw new Exception("Bucket Id is out of bounds");
                    }
                    buckets[bucketInt].Count++;
                    buckets[bucketInt].BBox = Geometry.Geometry.Union(buckets[bucketInt].BBox, Shapes[i].getBoundingBox());
                }

                //compute costs for splitting after each bucket
                double[] Costs = new double[buckets.Length - 1];
                int[] Counts = new int[buckets.Length - 1];
                Parallel.For(0, Costs.Length, index =>
                {
                    BoundingBox bBox1 = null, bBox2 = null;
                    int count1 = 0, count2 = 0;

                    for (int i = 0; i <= index; i++)
                    {
                        bBox1 = Geometry.Geometry.Union(bBox1, buckets[i].BBox);
                        count1 += buckets[i].Count;
                    }

                    for (int i = index + 1; i < buckets.Length; i++)
                    {
                        bBox2 = Geometry.Geometry.Union(bBox2, buckets[i].BBox);
                        count2 += buckets[i].Count;
                    }

                    double area1 = 0, area2 = 0;

                    if (bBox1 != null)
                    {
                        area1 = bBox1.GetSurfaceArea();
                        
                    }

                    if (bBox2 != null)
                    {
                        area2 = bBox2.GetSurfaceArea();
                    }

                    // Calculate costs
                    Costs[index] = .125 + (count1 * area1 + count2 * area2) / totalBounds.GetSurfaceArea();
                    Counts[index] = count1;
                });

                // Get  cheapest bucket
                double minCost = Costs[0];
                int minCostBucketID = 1;

                for (int i = 0; i < Costs.Length; i++)
                {
                    if (Costs[i] < minCost)
                    {
                        minCost = Costs[i];
                        minCostBucketID = i;
                    }
                }


                //either create leaf or split pirmitives at seleted sah bucket
                Parallel.For(0, 2, index =>
                {
                    int startIndex = StartNode;
                    int endIndex = StartNode + Counts[minCostBucketID] - 1;

                    if (index == 1)
                    {
                        startIndex = endIndex + 1;
                        endIndex = EndNode;
                    }
                    Children[index] = BuildRecurse(startIndex, endIndex, (int)axis);
                });

            }

            if (Children[0] != null && Children[1] != null)
            {
                node = new BVHNode(Children[0], Children[1], Geometry.Geometry.Union(Children[0].BBox, Children[1].BBox));
            }
            else if (Children[0] != null)
            {
                node = Children[0];
            }
            else
            {
                node = Children[1];
            }

            return node;
        }
    }
}
