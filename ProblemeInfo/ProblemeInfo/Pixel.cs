using System;
using System.Drawing;

namespace ProblemeInfo
{
    public class Pixel
    {
        byte r;
        byte g;
        byte b;
        #region Constructeur
        public Pixel(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
        #endregion
        #region getter setter
        public byte R 
        { 
            get { return r; }
            set { r = value; }
        }
        public byte G 
        { 
            get { return g; }
            set { g = value; }
        }
        public byte B 
        { 
            get { return b; }
            set { b = value; }
        }
        #endregion
        #region Moyenne
        /// <summary>
        /// On fais la moyenne pour le niveau de gris
        /// </summary>
        /// <returns></returns>
        public byte moyenne()
        {
            //Ces facteurs sont les plus souvent utilisés
            double moyenne = (this.r * 0.3 + this.g * 0.59 + this.b * 0.11);
            return Convert.ToByte(moyenne);
        }
        #endregion
        #region ToString
        /// <summary>
        /// On met en string le pixel
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.r + " " + this.g + " " + this.b;
        }
        #endregion
    }
}
