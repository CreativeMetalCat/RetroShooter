using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RetroShooter.Engine.Material
{
    public enum BasicVariableTypes
        {
            Bool,
            Float,
            FloatArray,
            Int,
            IntArray,
            Texture,
            Matrix,
            MatrixArray,
            Quaternion,
            Vector2,
            Vector2Array,
            Vector3,
            Vector3Array,
            Vector4,
            Vector4Array,
        }
        
        public class MatVariable
        {
            public string Name;

            public bool BoolValue;
            public float FloatValue;
            public float[] FloatArrayValue;
            public int IntValue;
            public int[] IntArrayValue;
            public Texture TextureValue;
            public Matrix MatrixValue;
            public Matrix[] MatrixArrayValue;
            public Quaternion QuaternionValue;
            public Vector2 Vector2Value;
            public Vector2[] Vector2ArrayValue;
            public Vector3 Vector3Value;
            public Vector3[] Vector3ArrayValue;
            public Vector4 Vector4Value;
            public Vector4[] Vector4ArrayValue;

            public BasicVariableTypes Type;

            public MatVariable(string name, bool value)
            {
                BoolValue = value;
                Name = name;
                Type = BasicVariableTypes.Bool;
            }

            public MatVariable(string name, int value)
            {
                IntValue = value;
                Name = name;
                Type = BasicVariableTypes.Int;
            }
            public MatVariable(string name, int[] value)
            {
                IntArrayValue = value ?? throw new NullReferenceException();
                Name = name;
                Type = BasicVariableTypes.IntArray;
            }
            public MatVariable(string name, float value)
            {
                FloatValue = value;
                Name = name;
                Type = BasicVariableTypes.Float;
            }
            
            public MatVariable(string name, float[] value)
            {
                FloatArrayValue = value ?? throw new NullReferenceException();
                Name = name;
                Type = BasicVariableTypes.FloatArray;
            }
            
            public MatVariable(string name, Texture value)
            {
                TextureValue = value ?? throw new NullReferenceException();
                Name = name;
                Type = BasicVariableTypes.Texture;
            }
            public MatVariable(string name,Matrix value)
            {
                MatrixValue = value;
                Name = name;
                Type = BasicVariableTypes.Matrix;
            }
            public MatVariable(string name,Matrix[] value)
            {
                MatrixArrayValue = value ?? throw new NullReferenceException();
                Name = name;
                Type = BasicVariableTypes.MatrixArray;
            }
            public MatVariable(string name, Quaternion value)
            {
                QuaternionValue = value;
                Name = name;
                Type = BasicVariableTypes.Quaternion;
            }
            public MatVariable(string name, Vector2 value)
            {
                Vector2Value = value;
                Name = name;
                Type = BasicVariableTypes.Vector2;
            }
            public MatVariable(string name,Vector2[] value)
            {
               Vector2ArrayValue = value ?? throw new NullReferenceException();
                Name = name;
                Type = BasicVariableTypes.Vector2Array;
            }
            public MatVariable(string name, Vector3 value)
            {
                Vector3Value = value;
                Name = name;
                Type = BasicVariableTypes.Vector3;
            }
            public MatVariable(string name, Vector3[] value)
            {
                Vector3ArrayValue = value ?? throw new NullReferenceException();
                Name = name;
                Type = BasicVariableTypes.Vector3Array;
            }
            public MatVariable(string name, Vector4 value)
            {
                Vector4Value = value;
                Name = name;
                Type = BasicVariableTypes.Vector4;
            }
            public MatVariable(string name, Vector4[] value)
            {
                Vector4ArrayValue = value ?? throw new NullReferenceException();
                Name = name;
                Type = BasicVariableTypes.Vector4Array;
            }
        }
}