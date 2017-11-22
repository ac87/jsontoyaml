using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MbJsonToYaml
{
    public class SpriteConverter
    {
        public static List<Sprite> GetSprites(string jsonFile)
        {
            JsonTextReader reader = new JsonTextReader(new StringReader(File.ReadAllText(jsonFile)));

            SpriteProp prop = SpriteProp.NotSet;
            Sprite sprite = new Sprite();

            List<Sprite> sprites = new List<Sprite>();

            while (reader.Read())
            {                
                if (reader.Value != null)
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        switch (reader.Value.ToString())
                        {
                            case "width":
                                prop = SpriteProp.Width;
                                break;
                            case "height":
                                prop = SpriteProp.Height;
                                break;
                            case "x":
                                prop = SpriteProp.X;
                                break;
                            case "y":
                                prop = SpriteProp.Y;
                                break;
                            case "pixelRatio":
                                prop = SpriteProp.PixelRatio;
                                break;
                            default:
                                sprite.Name = reader.Value.ToString();
                                break;                                
                        }
                    }
                    else
                    {
                        if (prop != SpriteProp.NotSet)
                        {
                            switch (prop)
                            {
                                case SpriteProp.Width:
                                    sprite.Width = int.Parse(reader.Value.ToString()) - 2;
                                    break;
                                case SpriteProp.Height:
                                    sprite.Height = int.Parse(reader.Value.ToString()) - 2;
                                    break;
                                case SpriteProp.X:
                                    sprite.X = int.Parse(reader.Value.ToString()) + 1;
                                    break;
                                case SpriteProp.Y:
                                    sprite.Y = int.Parse(reader.Value.ToString()) + 1;
                                    break;
                                case SpriteProp.PixelRatio:
                                    sprites.Add(sprite);
                                    sprite = new Sprite();
                                    prop = SpriteProp.NotSet;
                                    break;                                
                            }
                        }
                    }
                    //Console.WriteLine("Token: {0}, Value: {1}", reader.TokenType, reader.Value);
                }
            }

            return sprites;            
        }

        public class Sprite
        {
            public string Name { get; set; }

            public int Width { get; set; }

            public int Height { get; set; }

            public int X { get; set; }

            public int Y { get; set; }

            public int PixelRatio { get; set; }
        }

        public enum SpriteProp
        {
            NotSet,
            Width,
            Height,
            X,
            Y,
            PixelRatio
        }
    }
}
