using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RetroShooter.Engine.Material
{


    /**
     * Material handles loading data from material file into the game and handles applying of the default variables
     */
    public sealed  class Material
    {
       
        public List<MatVariable> Variables = new List<MatVariable>();

        private Effect _effect;

        private EffectParameter _pointLightColors;
        private EffectParameter _pointLightsLocation;
        private EffectParameter _pointLightsIntensity;
        private EffectParameter _pointLightsRadius;

        private EffectParameter _spotLightsColor;
        private EffectParameter _spotLightsLocation;
        private EffectParameter _spotLightsDirection;
        private EffectParameter _spotLightsIntensity;
        private EffectParameter _spotLightsRadius;
        private EffectParameter _spotLightsInnerCutoff;
        private EffectParameter _spotLightsOuterCutoff;
        
        public Effect Effect
        {
            get => _effect;
        }

        /*
         * This construction is only meant to be used if the material is created during LoadContent fase (aka only if it's called by object that is already loading)
         */
        public Material(string filename, RetroShooterGame game)
        {
            Load(filename,game);
        }

        public Material(){}

        /**
         * Loads all of the nessesary data from .mat file and stores it
         */
        public void Load(string materialName, RetroShooterGame game)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("./Content/Materials/" + materialName + ".mat");
            try
            {
                //TODO: Fix possible null reference
                var effectName = doc.DocumentElement.SelectSingleNode("/Material/EffectName").InnerText;

                if (effectName != "BasicEffect")
                {
                    _effect = game.Content.Load<Effect>("Effects/"+effectName);
                    //save variables for faster access
                    //point lights
                    _pointLightColors = _effect.Parameters["pointLightsColor"];
                    _pointLightsLocation = _effect.Parameters["pointLightsLocation"];
                    _pointLightsIntensity = _effect.Parameters["pointLightsIntensity"];
                    _pointLightsRadius = _effect.Parameters["pointLightsRadius"];

                    //spotlights
                    _spotLightsColor = _effect.Parameters["spotLightsColor"];
                    _spotLightsLocation = _effect.Parameters["spotLightsLocation"];
                    _spotLightsDirection = _effect.Parameters["spotLightsDirection"];
                    _spotLightsIntensity = _effect.Parameters["spotLightsIntensity"];
                    _spotLightsRadius = _effect.Parameters["spotLightsRadius"];
                    _spotLightsInnerCutoff = _effect.Parameters["spotLightsInnerCutoff"];
                    _spotLightsOuterCutoff = _effect.Parameters["spotLightsOuterCutoff"];
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
                                var mat = new MatVariable(texture.Attributes["type"]
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
                            Debug.Print(e.Message);
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

        /**
         * Applies all of the loaded data
         */
        public void Apply(RetroShooterGame game, Matrix objectTransform)
        {
            if (_effect != null)
            {
                //set basic values
                _effect.Parameters["World"]?.SetValue(game.CurrentCamera.WorldMatrix * objectTransform);
                _effect.Parameters["View"]?.SetValue(game.CurrentCamera.ViewMatrix);
                _effect.Parameters["Projection"]?.SetValue(game.CurrentCamera.ProjectionMatrix);
                _effect.Parameters["WorldInverseTranspose"]
                    ?.SetValue(Matrix.Transpose(Matrix.Invert(game.CurrentCamera.WorldMatrix * objectTransform)));

                _effect.Parameters["AmbientLightColor"]?.SetValue(game.CurrentAmbientLightColor);


                if (game.PointLightsDirty)
                {
                    _pointLightColors?.SetValue(game.PointColors);
                    _pointLightsLocation?.SetValue(game.PointLocations);
                    _pointLightsIntensity?.SetValue(game.PointIntensities);
                    _pointLightsRadius?.SetValue(game.PointRadii);
                }

                if (game.SpotlightsDirty)
                {
                    _spotLightsColor?.SetValue(game.SpotColors);
                    _spotLightsLocation?.SetValue(game.SpotLocations);
                    _spotLightsIntensity?.SetValue(game.SpotIntensities);
                    _spotLightsRadius?.SetValue(game.SpotRadii);
                    _spotLightsDirection?.SetValue(game.SpotDirections);
                    _spotLightsOuterCutoff?.SetValue(game.SpotInnerCutoffs);
                    _spotLightsInnerCutoff?.SetValue(game.SpotOuterCutoffs);
                }
                
                foreach (MatVariable variable in Variables)
                {
                    switch (variable.Type)
                    {
                        case BasicVariableTypes.Bool:
                            _effect.Parameters[variable.Name]?.SetValue(variable.BoolValue);
                            break;
                        case BasicVariableTypes.FloatArray:
                            _effect.Parameters[variable.Name]?.SetValue(variable.FloatArrayValue);
                            break;
                        case BasicVariableTypes.Float:
                            _effect.Parameters[variable.Name]?.SetValue(variable.FloatValue);
                            break;
                        case BasicVariableTypes.Int:
                            _effect.Parameters[variable.Name]?.SetValue(variable.IntValue);
                            break;
                        case BasicVariableTypes.IntArray:
                            _effect.Parameters[variable.Name]?.SetValue(variable.IntArrayValue);
                            break;
                        case BasicVariableTypes.Quaternion:
                            _effect.Parameters[variable.Name]?.SetValue(variable.QuaternionValue);
                            break;
                        case BasicVariableTypes.Texture:
                            _effect.Parameters[variable.Name]?.SetValue(variable.TextureValue);
                            break;
                        case BasicVariableTypes.Matrix:
                            _effect.Parameters[variable.Name]?.SetValue(variable.MatrixValue);
                            break;
                        case BasicVariableTypes.MatrixArray:
                            _effect.Parameters[variable.Name]?.SetValue(variable.MatrixArrayValue);
                            break;
                        case BasicVariableTypes.Vector2:
                            _effect.Parameters[variable.Name]?.SetValue(variable.Vector2Value);
                            break;
                        case BasicVariableTypes.Vector2Array:
                            _effect.Parameters[variable.Name]?.SetValue(variable.Vector2ArrayValue);
                            break;
                        case BasicVariableTypes.Vector3Array:
                            _effect.Parameters[variable.Name]?.SetValue(variable.Vector3ArrayValue);
                            break;
                        case BasicVariableTypes.Vector4:
                            _effect.Parameters[variable.Name]?.SetValue(variable.Vector4Value);
                            break;
                        case BasicVariableTypes.Vector4Array:
                            _effect.Parameters[variable.Name]?.SetValue(variable.Vector4ArrayValue);
                            break;
                        case BasicVariableTypes.Vector3:
                            _effect.Parameters[variable.Name]?.SetValue(variable.Vector3Value);
                            break;
                        default:
                            _effect.Parameters[variable.Name]?.SetValue(variable.BoolValue);
                            break;
                    }
                }

            }
        }

    }
}