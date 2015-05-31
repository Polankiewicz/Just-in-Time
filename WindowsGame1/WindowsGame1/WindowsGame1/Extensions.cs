using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    public static class Extensions
    {
        public static void SetEffectParameter(this Effect effect, string parameterName, object value)
        {
            var parameter = effect.Parameters[parameterName];

            if (parameter == null)
                return;
            if (value is bool)
                parameter.SetValue((bool)value);
            if (value is Vector3)
                parameter.SetValue((Vector3)value);
            if (value is Matrix)
                parameter.SetValue((Matrix)value);
            if (value is Texture2D)
                parameter.SetValue((Texture2D)value);
        }
    }
}
