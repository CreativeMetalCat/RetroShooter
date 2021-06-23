using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RetroShooter.Engine.Material
{
    
    
    /**
     * Material handles loading data from material file into the game and handles applying of the default variables
     */
    public class Material
    {
        public struct MatVariable
        {
            public string Name;
            public object Variable;

            public MatVariable(string _name, object variable)
            {
                Name = _name ?? throw new NullReferenceException(nameof(Name) + ". All effect parameters MUST NOT be null");
                Variable = variable ?? throw new NullReferenceException(nameof(variable)+". All effect parameters MUST NOT be null");
            }

        }
        
        public List<MatVariable> Variables = new List< MatVariable>();

        private Effect _effect;
        
        public  Effect Effect
        {
            get => _effect;
        }
        public void Load(string materialName, RetroShooterGame game)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("./Content/Materials/" + materialName + ".mat");
            try
            {
                var effectName = doc.DocumentElement.SelectSingleNode("/Material/EffectName").InnerText;
                
                if (effectName != "BasicEffect")
                {
                    _effect = game.Content.Load<Effect>(effectName);
                }

                var textures = doc.DocumentElement.SelectSingleNode("/Material/Textures");
                try
                {
                    foreach (XmlNode texture in textures.ChildNodes)
                    {
                        try
                        {
                            if (texture != null)
                            {
                                var mat = new MatVariable(texture["type"]
                                        .InnerText,
                                    /*if texture is null then it will throw null exception and be ignored*/
                                    game.Content.Load<Texture2D>(texture
                                        .InnerText));
                                //variable is created separately to avoid putting null variable into the material
                                Variables.Add(mat);
                            }
                        }
                        catch (NullReferenceException e)
                        {
                            //we just log it and move on
                            //TODO: Add log
                        }

                    }
                }
                catch (Exception e)
                {
                    // ignored
                }
            }
            catch (System.NullReferenceException e)
            {
                //TODO: Log about null shader asset
                _effect = new BasicEffect(game.GraphicsDevice);
            }
        }
    }
}