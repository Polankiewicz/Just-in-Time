using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.ComponentModel;
namespace WindowsGame1
{
    public class DrawableBoundingBox : INotifyPropertyChanged
    {
        private Vector3 _min;
        private Vector3 _max;
        private String _name;

        GraphicsDevice device;
        GeometricPrimitive primitive;
        BoundingBox _boundingBox;

        public event PropertyChangedEventHandler PropertyChanged;

        public Vector3 min
        {
            get { return _min; }
            set
            {
                _min = value;
                this.NotifyPropertyChanged("min");
            }
        }

        public Vector3 max
        {
            get { return _max; }
            set
            {
                _max = value;
                this.NotifyPropertyChanged("max");
            }
        }

        public BoundingBox boundingBox
        {
            get { return _boundingBox; }
            set { _boundingBox = value; }
        }

        public String name
        {
            get { return _name; }
            set { _name = value; }
        }

        public DrawableBoundingBox(GraphicsDevice device, Vector3 min, Vector3 max)
        {
            this.device = device;
            this.min = min;
            this.max = max;

            this.Calculate();
        }

        public void Calculate()
        {
            _boundingBox = new BoundingBox(this.min, this.max);
            primitive = new WireBox(this.device, this.min, this.max);
        }

        public void Draw(Camera camera)
        {
            primitive.Draw(camera);
        }

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
                this.Calculate();
            }
        }
    }
}
