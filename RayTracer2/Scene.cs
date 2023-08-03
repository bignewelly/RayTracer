using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RayTracer2.Geometry;
using RayTracer2.Illumination;
using RayTracer2.BVH;

namespace RayTracer2
{
    public class Scene
    {
        public Scene(IShape[] Shapes, List<ILight> Lights)
        {
            this.BVH = new BoundingVolumeHiarchy(Shapes);
            this.Lights = Lights;
        }



        public Color Intersect(Ray r, int RelfectionDepth = 4)
        {
            Color retVal = new Color();

            HitRecord bestRecord = Hit(r);

            // If we have a record, that's the color we want.
            if (bestRecord != null)
            {
                Point IntersectionPoint = Geometry.Geometry.GetPositionOnRay(r, bestRecord.Time);
                switch (bestRecord.Material.surfaceType)
                {

                    case SurfaceType.Diffuse:
                        {
                            foreach (ILight light in Lights)
                            {
                                Color lightColor = light.GetColor();
                                Color objectColor = bestRecord.Material.color;

                                Vector lightDirection = Geometry.Geometry.Normalize(light.GetPosition() - IntersectionPoint);
                                Ray l = new Ray(IntersectionPoint, lightDirection);

                                // Check for shadows
                                HitRecord shadowRecord = Hit(l);

                                if (!(shadowRecord != null && shadowRecord.Time > 0 && shadowRecord.Time < Geometry.Geometry.Distance(light.GetPosition(), Geometry.Geometry.GetPositionOnRay(l, shadowRecord.Time))))
                                {
                                    Color tempCol = new Color();

                                    tempCol = objectColor * light.GetColor() * Geometry.Geometry.Dot(lightDirection, bestRecord.Normal);

                                    retVal += tempCol;

                                }

                            }
                            break;
                        };
                    case SurfaceType.Reflective:
                        {
                            if (RelfectionDepth > 0)
                            {
                                // get relfection ray 
                                Vector Direction = r.Direction - 2 * (Geometry.Geometry.Dot(r.Direction, bestRecord.Normal) * bestRecord.Normal);
                                Ray newRay = new Ray(IntersectionPoint, Direction);

                                // Cast new ray into the scene and decriment the depth;
                                retVal = Intersect(newRay, RelfectionDepth - 1) * bestRecord.Material.color;
                            } else
                            {
                                //return black.
                                retVal = new Color();
                            }
                            break;
                        }
                    case SurfaceType.Refractive:
                        {

                            if (RelfectionDepth > 0)
                            {
                                Ray newRay;


                                if (r.CurrentRefractionIndex != bestRecord.Material.RefractiveIndex)
                                {
                                    double ratio;
                                    double cos = Geometry.Geometry.Dot(r.Direction, bestRecord.Normal);

                                    if (cos < 0)
                                    {
                                        ratio = r.CurrentRefractionIndex / bestRecord.Material.RefractiveIndex;
                                    }
                                    else
                                    {
                                        ratio = bestRecord.Material.RefractiveIndex / r.CurrentRefractionIndex;
                                    }

                                    double sinSqrd = (ratio * ratio) * (1 - (cos * cos));
                                    double sqrRoot = Math.Sqrt(1 - sinSqrd);

                                    // get refraction ray 
                                    Vector Direction = ratio * r.Direction + ((ratio * cos) - sqrRoot) * bestRecord.Normal;


                                    if (cos < 0)
                                    {
                                        newRay = new Ray(IntersectionPoint, Direction, bestRecord.Material.RefractiveIndex);
                                    }
                                    else
                                    {
                                        newRay = new Ray(IntersectionPoint, Direction);
                                    }
                                } else
                                {
                                    newRay = new Ray(IntersectionPoint, r.Direction, r.CurrentRefractionIndex);
                                }


                                // Cast new ray into the scene and decriment the depth;
                                retVal = Intersect(newRay, RelfectionDepth - 1) * bestRecord.Material.color;

                            }
                            else
                            {
                                //return black.
                                retVal = new Color();
                            }
                            break;
                        }
                }

            }

            return retVal;
        }

        public HitRecord Hit(Ray r)
        {
            IShape intersectedShape = BVH.getIntersectedShape(r);

            HitRecord bestRecord = null;

            if (intersectedShape != null)
            {
                bestRecord = intersectedShape.Hit(r);
            }

            return bestRecord;
        }

        public void AddLight(ILight Light)
        {
            Lights.Add(Light);
        }

        public void AddLights(List<ILight> Lights)
        {
            this.Lights.AddRange(Lights);
        }

        private BoundingVolumeHiarchy BVH;
        private List<ILight> Lights;
        
    }
}
