using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using RayTracer2.Geometry;

namespace RayTracer2.Parsers
{
    enum Format
    {
        ASCII1_0,
        BinaryLittleEndian1_0,
        BinaryBigEndian1_0
    }

    public class PlyParser
    {
        const string ASCII1_0_String = "ascii 1.0";
        const string Binary_Little_Endian_1_0_String = "binary_little_endian 1.0";
        const string Binary_Big_Endian_1_0_String = "binary_big_endian 1.0";
        private Format _Format = Format.ASCII1_0;

        public IShape Parse(String FileName, Material mat, Vector Offest = null)
        {
            string[] lines = File.ReadAllLines(FileName);

            if (lines.Length > 0)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(lines[i]))
                    {
                        switch (lines[i].Trim())
                        {
                            case "ply":
                                return ParsePly(lines, i + 1, mat, Offest);
                        }
                        
                    }
                }
            }

            return null;
        }

        public IShape ParsePly(string[] Lines, int StartingAt, Material mat, Vector offset = null)
        {
            List<PlyElement> Elements = new List<PlyElement>();
            List<PlyObjectInfo> Info = new List<PlyObjectInfo>();

            Boolean isHeader = true;
            int ValueStartIndex = 0;

            for (int i = StartingAt; i < Lines.Length && isHeader; i++)
            {
                if (!Lines[i].StartsWith("comment"))
                {
                    if (Lines[i].StartsWith("format"))
                    {
                        switch (Lines[i].Replace("format", "").Trim().ToLower())
                        {
                            case ASCII1_0_String:
                                _Format = Format.ASCII1_0;
                                break;
                            case Binary_Little_Endian_1_0_String:
                                _Format = Format.BinaryLittleEndian1_0;
                                break;
                            case Binary_Big_Endian_1_0_String:
                                _Format = Format.BinaryBigEndian1_0;
                                break;
                        }
                    }
                    else if (Lines[i].StartsWith("element"))
                    {
                        PlyElement element = new PlyElement();
                        String subString = Lines[i].Replace("element", "").Trim();
                        element.Name = subString.Substring(0, subString.IndexOf(" ")).Trim();
                        element.Count = int.Parse(Lines[i].Replace("element", "").ToLower().Replace(element.Name, "").Trim());

                        i++;
                        element.Properties = GetProperties(Lines, ref i);
                        i--;

                        Elements.Add(element);
                    } else if (Lines[i].StartsWith("end_header"))
                    {
                        ValueStartIndex = i + 1;
                        break;
                    }
                }
                ValueStartIndex = i;
            }

            Point[] vertices = new Point[Elements[0].Count];

            Parallel.For(ValueStartIndex, ValueStartIndex + Elements[0].Count,
                index =>
                {
                    if (_Format != Format.ASCII1_0)
                    {
                        throw new Exception(String.Format("{0} is not supported.", _Format));
                    }

                    String line = Lines[index];

                    Point vertex = new Point(0, 0, 0);
                    for (int k = 0; k < Elements[0].Properties.Count(); k++)
                    {
                        switch (Elements[0].Properties[k].Value.ToLower())
                        {
                            case "x":

                                if (line.Contains(" "))
                                {
                                    vertex.x = Double.Parse(line.Substring(0, line.IndexOf(" ")));
                                }
                                else
                                {
                                    vertex.x = Double.Parse(line);
                                }

                                break;
                            case "y":
                                if (line.Contains(" "))
                                {
                                    vertex.y = Double.Parse(line.Substring(0, line.IndexOf(" ")));
                                }
                                else
                                {
                                    vertex.y = Double.Parse(line);
                                }

                                break;
                            case "z":
                                if (line.Contains(" "))
                                {
                                    vertex.z = Double.Parse(line.Substring(0, line.IndexOf(" ")));
                                }
                                else
                                {
                                    vertex.z = Double.Parse(line);
                                }

                                break;
                        }

                        line = line.Substring(line.IndexOf(" ") + 1, line.Length - line.IndexOf(" ") - 1);

                    }

                    vertices[index - ValueStartIndex] = vertex;
                });

            ValueStartIndex += Elements[0].Count;

            Triangle[] triangles = new Triangle[Elements[1].Count];
            Parallel.For(ValueStartIndex, ValueStartIndex + Elements[1].Count,
                index =>
                {
                    String line = Lines[index];

                    int count = int.Parse(line.Substring(0, line.IndexOf(" ")));

                    Point[] verts = new Point[count];

                    for (int i = 0; i < count; i++)
                    {
                        if (line.Trim().Contains(" "))
                        {
                            line = line.Substring(line.IndexOf(" "), line.Length - line.IndexOf(" ")).Trim();
                        }

                        if (line.Contains(" "))
                        {
                            verts[i] = vertices[int.Parse(line.Substring(0, line.IndexOf(" ")))];
                        } else
                        {
                            verts[i] = vertices[int.Parse(line)];
                        }
                    }

                    triangles[index - ValueStartIndex] = new Triangle(verts[0], verts[1], verts[2]);
                });

            return new Mesh(triangles, mat, offset);

        }

        public List<PlyProperty> GetProperties(string[] Lines, ref int StartingAt)
        {
            List<PlyProperty> Properties = new List<PlyProperty>();
            for (int i = StartingAt; i < Lines.Length; i++)
            {
                if (Lines[i].StartsWith("property"))
                {
                    String subString = Lines[i].Replace("property", "").Trim();
                    PlyProperty property = new PlyProperty();
                    property.PropertyType = subString.Substring(0, subString.IndexOf(" "));
                    property.Value = subString.Substring(subString.IndexOf(" "), subString.Length - subString.IndexOf(" ")).Trim();

                    Properties.Add(property);
                } else
                {
                    break;
                }

                StartingAt = i;
            }

            return Properties;
        }

        private Byte[] GetBytesFromBinaryString(String binary)
        {
            List<Byte> bytes = new List<byte>();

            for (int i = 0; i < binary.Length; i += 4)
            {
                string s = binary.Substring(i, 4);

                bytes.Add(Convert.ToByte(s, 2));
            }

            return bytes.ToArray();
        }
    }

    public class PlyElement
    {
        public String Name;
        public List<PlyProperty> Properties;
        public int Count = 0;
    }

    public class PlyProperty
    {
        public String PropertyType;
        public String Value;
    }

    public class PlyObjectInfo
    {
        public String Name;
        public String Value;
    }

}
