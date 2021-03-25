using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.ContentStorage
{
    class Effects
    {
        ContentManager content;
        private Dictionary<EffectType, Effect> effectContentList;

        public Effects(ContentManager content)
        {
            this.content = content;
            effectContentList = new Dictionary<EffectType, Effect>();
            Load();
        }

        private void Load()
        {
            effectContentList.Add(EffectType.Lighting, content.Load<Effect>(@"Effects/Lighting"));
            effectContentList.Add(EffectType.Shadow, content.Load<Effect>(@"Effects/Shadow"));
            effectContentList.Add(EffectType.ShadowInverter, content.Load<Effect>(@"Effects/ShadowInverter"));

            InitBlur();
            InitSoften();


        }

        private void InitBlur()
        {
            Effect blur = content.Load<Effect>(@"Effects/Blur");
            float[] weights = { 0.1061154f, 0.1028506f, 0.1028506f, 0.09364651f, 0.09364651f, 0.0801001f, 0.0801001f, 0.06436224f, 0.06436224f, 0.04858317f, 0.04858317f, 0.03445063f, 0.03445063f, 0.02294906f, 0.02294906f };
            float[] offsets = { 0, 0.00125f, -0.00125f, 0.002916667f, -0.002916667f, 0.004583334f, -0.004583334f, 0.00625f, -0.00625f, 0.007916667f, -0.007916667f, 0.009583334f, -0.009583334f, 0.01125f, -0.01125f };
            for (int i = 0; i < weights.Length; i++)
            {
                offsets[i] *= 0.6f;
            }
            blur.Parameters["weights"].SetValue(weights);
            blur.Parameters["offsets"].SetValue(offsets);
            effectContentList.Add(EffectType.Blur, blur);
        }

        private void InitSoften()
        {
            Effect soften = content.Load<Effect>(@"Effects/Soften");
            float[] weights = { 0.1061154f, 0.1028506f, 0.1028506f, 0.09364651f};
            float[] offsets = { 0, 0.00125f, -0.00125f, 0.002916667f, -0.002916667f};
            for (int i = 0; i < weights.Length; i++)
            {
                offsets[i] *= 0.6f;
            }
         //   soften.Parameters["weights"].SetValue(weights);
         //   soften.Parameters["offsets"].SetValue(offsets);
            effectContentList.Add(EffectType.Soften, soften);
        }

        public Effect GetEffect(EffectType effectType)
        {
            return effectContentList[effectType];
        }
    }

    enum EffectType
    {
        LightingAndShadow,
        Lighting,
        Shadow,
        ShadowInverter,
        Blur,
        Soften
    }
}

