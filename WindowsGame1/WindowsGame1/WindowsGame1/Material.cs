﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    public class Material
    {
        public virtual void SetEffectParameters(Effect effect)
        {
        }
    }
    public class LightingMaterial : Material
    {
        public Vector3 AmbientColor { get; set; }
        public Vector3 LightDirection { get; set; }
        public Vector3 LightColor { get; set; }
        public Vector3 SpecularColor { get; set; }
        public LightingMaterial()
        {
            AmbientColor = new Vector3(.1f, .1f, .1f);
            LightDirection = new Vector3(1, 1, 1);
            LightColor = new Vector3(.9f, .9f, .9f);
            SpecularColor = new Vector3(1, 1, 1);
        }
        public override void SetEffectParameters(Effect effect)
        {
            effect.SetEffectParameter("AmbientColor", AmbientColor);
            effect.SetEffectParameter("LightColor", LightColor);
            effect.SetEffectParameter("LightDirection", LightDirection);
            effect.SetEffectParameter("SpecularColor", SpecularColor);
        }
    }
}
