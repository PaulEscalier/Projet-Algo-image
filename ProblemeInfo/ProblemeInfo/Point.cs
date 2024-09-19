using System;

namespace ProblemeInfo
{
    public class Point
    {
        double x;
        double y;
        public Point(double x,double y)
        {
            this.x = x;
            this.y = y;
        }
        public double X
        {
            get { return x; }
            set { x = value; }
        }
        public double Y
        {
            get { return y; }
            set { y = value; }
        }
        /// <summary>
        /// On met le complexe au carré
        /// </summary>
        public void Carre()
        {
            double Xmem = x;
            x = x * x - y * y;
            y = 2 * Xmem * y;
        }
        /// <summary>
        /// La norme du complexe
        /// </summary>
        /// <returns>La norme en double</returns>
        public double Norme()
        {
            return Math.Sqrt(x * x + y * y);
        }
    }
}
