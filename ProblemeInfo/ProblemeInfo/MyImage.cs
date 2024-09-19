using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace ProblemeInfo
{
    public class MyImage
    {
        Pixel[,] image;
        
        int BitsParCouleur;
        byte[] myfile;
        string type;

        long TailleFile;
        long TailleOffset;
        long tailleInfoHeader;

        long largeur;
        long hauteur;
        
        long planes;
        long compression;
        long imageSize;
        bool aPartirDunFichier;
        #region Constructeur
        /// <summary>
        /// Ici le but est de construire l'image, toutes les informations seront prises dans la classe pour une plus grande adaptabilité
        /// </summary>
        /// <param name="nomFichier">Le nom du fichier que vous voulez implémenter dans la classe</param>
        public MyImage(string nomFichier, bool aPartirDunFichier)
        {
            this.aPartirDunFichier = aPartirDunFichier;
            if(aPartirDunFichier)
            {
                this.myfile = File.ReadAllBytes(nomFichier + ".bmp");

                byte[] tableauDecryptage;

                this.type += Convert.ToChar(this.myfile[0]) + "" + Convert.ToChar(this.myfile[1]);

                tableauDecryptage = new byte[] { this.myfile[2], this.myfile[3], this.myfile[4], this.myfile[5] };
                this.TailleFile = Convertir_Endian_To_Int(tableauDecryptage);

                tableauDecryptage = new byte[] { this.myfile[10], this.myfile[11], this.myfile[12], this.myfile[13] };
                this.TailleOffset = Convertir_Endian_To_Int(tableauDecryptage);

                tableauDecryptage = new byte[] { this.myfile[14], this.myfile[15], this.myfile[16], this.myfile[17] };
                this.tailleInfoHeader = Convertir_Endian_To_Int(tableauDecryptage);

                tableauDecryptage = new byte[] { this.myfile[18], this.myfile[19], this.myfile[20], this.myfile[21] };
                this.largeur = Convertir_Endian_To_Int(tableauDecryptage);

                tableauDecryptage = new byte[] { this.myfile[22], this.myfile[23], this.myfile[24], this.myfile[25] };
                this.hauteur = Convertir_Endian_To_Int(tableauDecryptage);

                tableauDecryptage = new byte[] { this.myfile[26], this.myfile[27] };
                this.planes = Convertir_Endian_To_Int(tableauDecryptage);

                tableauDecryptage = new byte[] { this.myfile[28], this.myfile[29] };
                this.BitsParCouleur = Convertir_Endian_To_Int(tableauDecryptage);

                tableauDecryptage = new byte[] { this.myfile[30], this.myfile[31], this.myfile[32], this.myfile[33] };
                this.compression = Convertir_Endian_To_Int(tableauDecryptage);

                tableauDecryptage = new byte[] { this.myfile[34], this.myfile[35], this.myfile[36], this.myfile[37] };
                this.imageSize = Convertir_Endian_To_Int(tableauDecryptage);

                this.image = new Pixel[hauteur, largeur];

                long pointeur = this.TailleOffset;

                for (int i = 0; i < this.hauteur; i++)
                {
                    for (int j = 0; j < this.largeur; j++)
                    {
                        this.image[i, j] = new Pixel(0, 0, 0);
                        for (int k = 0; k < 3; k++)
                        {
                            if (k == 0)
                                this.image[i, j].R = this.myfile[pointeur];
                            else if (k == 1)
                                this.image[i, j].G = this.myfile[pointeur];
                            else if (k == 2)
                                this.image[i, j].B = this.myfile[pointeur];
                            pointeur++;
                        }
                    }

                }
            }
            else
            {
                this.type = "BM";
                this.TailleFile = 0;
                this.TailleOffset = 54;
                this.tailleInfoHeader = 40;
                this.hauteur = 0;
                this.largeur = 0;
                this.planes = 1;
                this.BitsParCouleur = 24;
                this.compression = 0;
                this.imageSize = 0;
            }
            
        }
        #endregion
        #region ToString
        /// <summary>
        /// On transforme notre tableau de Pixels en String pour l'afficher
        /// </summary>
        /// <returns></returns>
        public override string ToString() //Ne sert à rien
        {
            string retour = "";
            for (int i = 0; i < this.hauteur; i++)
            {
                for (int j = 0; j < this.largeur; j++)
                {
                    retour += this.image[i, j].ToString() + " |";
                }
                retour += "\n";
            }
            return retour;
        }
        #endregion
        #region From_Image_To_File
        /// <summary>
        /// On crée un fichier image à partir des élément de l'objet de classe
        /// </summary>
        /// <param name="file">Le nom que vous voulez donner à ce fichier</param>
        public void From_Image_To_File(string file)
        {
            this.hauteur = this.image.GetLength(0);
            this.largeur = this.image.GetLength(1);
            long index = 0;
            byte[] FichierBMP = new byte[3 * (this.hauteur * this.largeur) + 54];
            this.TailleFile = FichierBMP.Length;
            #region Header
            for (int i=0;i<2;i++)
            {
                FichierBMP[index] = Convert.ToByte(this.type[i]);
                index++;
            }
                


            byte[] FileSize = Convertir_Int_To_Endian(this.TailleFile, 4);
            for (int i = 0; i < FileSize.Length; i++)
            {
                FichierBMP[index] = FileSize[i];
                index++;
            }
                


            for (int i = 0; i < 4; i++)
            {
                FichierBMP[index] = 0;
                index++;
            }


            byte[] FileOffset = Convertir_Int_To_Endian(this.TailleOffset, 4);
            for (int i = 0; i < FileOffset.Length; i++)
            {
                FichierBMP[index] = FileOffset[i];
                index++;
            }
                


            #endregion
            #region InfoHeader

            byte[] tailleInfoHeader=Convertir_Int_To_Endian(this.tailleInfoHeader, 4);
            for (int i = 0; i < tailleInfoHeader.Length; i++)
            {
                FichierBMP[index] = tailleInfoHeader[i];
                index++;
            }
                

            byte[] largeur = Convertir_Int_To_Endian(this.largeur,4);
            for (int i = 0; i < largeur.Length; i++)
            {
                FichierBMP[index] = largeur[i];
                index++;
            }
                

            byte[] hauteur = Convertir_Int_To_Endian(this.hauteur, 4);
            for (int i = 0; i < hauteur.Length; i++)
            {
                FichierBMP[index] = hauteur[i];
                index++;
            }
                

            byte[] planes = Convertir_Int_To_Endian(this.planes, 2);
            for (int i = 0; i < planes.Length; i++)
            {
                FichierBMP[index] = planes[i];
                index++;
            }
                

            byte[] BitsParCouleur = Convertir_Int_To_Endian(this.BitsParCouleur, 2);
            for (int i = 0; i < BitsParCouleur.Length; i++)
            {
                FichierBMP[index] = BitsParCouleur[i];
                index++;
            }
                

            byte[] Compression = Convertir_Int_To_Endian(this.compression, 4);
            for (int i = 0; i < Compression.Length; i++)
            {
                FichierBMP[index] = Compression[i];
                index++;
            }
                

            byte[] ImageSize = Convertir_Int_To_Endian(this.imageSize, 4);
            for (int i = 0; i < ImageSize.Length; i++)
            {
                FichierBMP[index] = ImageSize[i];
                index++;
            }
                

            for (int i = 0; i < 16; i++)
            {
                FichierBMP[index] = 0;
                index++;
            }
                
            #endregion
            for (int i = 0; i < this.hauteur; i++)
            {
                for (int k = 0; k < this.largeur; k++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (j == 0)
                            FichierBMP[index] = image[i, k].R;
                        else if (j == 1)
                            FichierBMP[index] = image[i, k].G;
                        else if (j == 2)
                            FichierBMP[index] = image[i, k].B;

                        index++;
                    }
                }
            }
            File.WriteAllBytes(file+".bmp", FichierBMP);
        }
        #endregion
        #region  Convertir_Endian_To_Int
        /// <summary>
        /// convertit une séquence d’octets au format little endian en entier
        /// </summary>
        /// <param name="tab">la tableau d'octets rangé selon le format little Endian à convertir</param>
        /// <returns></returns>
        public int Convertir_Endian_To_Int(byte[] tab)
        {
            int retour=0;
            for (int i = 0; i < tab.Length; i++)
            {
                retour += Convert.ToInt32(tab[i] * Math.Pow(256, i));

            }
            return retour;
        }
        #endregion
        #region Convertir_Int_To_Endian          
        /// <summary>
        /// convertit un entier en séquence d’octets au format little endian
        /// </summary>
        /// <param name="val">La valeur à convertir</param>
        /// <param name="nbB">le nombre d'octets sur laquelle cette valeur sera encodée</param>
        /// <returns></returns>
        public byte[] Convertir_Int_To_Endian(long val, int nbB)
        {
            double valeur = val;
            byte[] retour = new byte[nbB];
            for (int i = nbB - 1; i >= 0; i--)
            {
                while (valeur - Math.Pow(256, i) >= 0)
                {
                    retour[i] += 1;
                    valeur -= Math.Pow(256, i);
                }
            }
            return retour;
        }
        #endregion
        #region Convertir byte en binaire
        public int[] Convertir_byte_to_binary(byte val, int nbB)
        {
            double valeur = val;
            int[] retour = new int[nbB];
            for (int i = nbB - 1; i >= 0; i--)
            {
                if(valeur - Math.Pow(2, i) >= 0)
                {
                    retour[i] = 1;
                    valeur -= Math.Pow(2, i);
                }
                else
                {
                    retour[i] = 0;
                }
            }
            return retour;
        }

        #endregion
        #region Convertir Binaire en byte
        public byte Convertir_Binary_To_Byte(int[] tab)
        {
            int retour = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                retour += Convert.ToInt32(tab[i] * Math.Pow(2, i));

            }
            return Convert.ToByte(retour);
        }
        #endregion

        #region Quart supérieur gauche
        public void Quart()
        {
            this.largeur = this.largeur / 2;
            this.hauteur = this.hauteur / 2;

            Pixel[,] memoire = new Pixel[this.hauteur, this.largeur];
            for (int i = 0; i < this.hauteur; i++)
            {
                for (int k = 0; k < this.largeur; k++)
                {
                    memoire[i, k] =this.image[i + this.hauteur, k] ;
                }
            }
            this.image = memoire;
        }
        #endregion
        #region Niveau de gris
        public void Niveau_Gris()
        {
            for (int i = 0; i < this.hauteur; i++)
            {
                for (int k = 0; k < this.largeur; k++)
                {
                    byte moyenne = this.image[i, k].moyenne();
                    this.image[i, k].R = moyenne;
                    this.image[i, k].G = moyenne;
                    this.image[i, k].B = moyenne;
                }
            }
        }
        #endregion
        #region Rotation
        /// <summary>
        /// Fait faire une rotation à l'image
        /// </summary>
        /// <param name="degré">degré de rotation en degré</param>
        public void Rotation(double degré)
        {
            Console.WriteLine(degré);
            degré = (degré / 180.0) * Math.PI;
            Console.WriteLine(degré);
            Console.ReadLine();
            int x_min = 0;
            int x_max=0;
            int y_min = 0;
            int y_max = 0;
            for(int i = 0; i < this.hauteur; i++)
            {
                for(int j=0;j<this.largeur; j++)
                {
                    double nouveau_X = j * Math.Cos(degré) - i * Math.Sin(degré);
                    if (nouveau_X<x_min)
                    {
                        x_min=Convert.ToInt32(nouveau_X);
                    }else if(nouveau_X>x_max)
                    {
                        x_max=Convert.ToInt32(nouveau_X);
                    }

                    double nouveau_Y = j * Math.Sin(degré) + i * Math.Cos(degré);
                    if (nouveau_Y < y_min)
                    {
                        y_min = Convert.ToInt32(nouveau_Y);
                    }
                    else if (nouveau_Y > y_max)
                    {
                        y_max = Convert.ToInt32(nouveau_Y);
                    }
                }
            }
            int hauteur = y_max - y_min + 1;
            int largeur = x_max - x_min + 1;

            hauteur = hauteur + (4 - (hauteur % 4));
            largeur = largeur + (4 - (largeur % 4));



            Pixel[,] Nouvelle_Matrice = new Pixel[hauteur, largeur];

            for (int i = 0; i < Nouvelle_Matrice.GetLength(0); i++)
            {
                for (int j = 0; j < Nouvelle_Matrice.GetLength(1); j++)
                {
                    Nouvelle_Matrice[i, j] = new Pixel(0, 0, 0);
                }
            }



            for (int i = 0; i < Nouvelle_Matrice.GetLength(0); i++)
            {
                for (int j = 0; j < Nouvelle_Matrice.GetLength(1); j++)
                {
                    double ancien_X = (j + x_min) * Math.Cos(degré) + (i + y_min) * Math.Sin(degré);
                    double ancien_Y = (j + x_min) * -Math.Sin(degré) + (i + y_min) * Math.Cos(degré);

                    int ancien_X_arrondi = Convert.ToInt32(ancien_X);
                    int ancien_Y_arrondi = Convert.ToInt32(ancien_Y);

                    // Vérifier si les coordonnées correspondent à un pixel valide dans l'ancienne matrice
                    if (ancien_X_arrondi >= 0 && ancien_X_arrondi < this.largeur && ancien_Y_arrondi >= 0 && ancien_Y_arrondi < this.hauteur)
                    {
                        Nouvelle_Matrice[i, j] = this.image[ancien_Y_arrondi, ancien_X_arrondi];
                    }
                }
            }



            this.image = Nouvelle_Matrice;
            this.hauteur = Nouvelle_Matrice.GetLength(0);
            this.largeur = Nouvelle_Matrice.GetLength(1);
        }
        #endregion
        #region Noir et Blanc
        public void Noir_et_Blanc()
        {
            for (int i = 0; i < this.hauteur; i++)
            {
                for (int k = 0; k < this.largeur; k++)
                {
                    byte moyenne = this.image[i, k].moyenne();
                    if(moyenne > 128)
                    {
                        this.image[i, k].R = 255;
                        this.image[i, k].G = 255;
                        this.image[i, k].B = 255;
                    }
                    else
                    {
                        this.image[i, k].R = 0;
                        this.image[i, k].G = 0;
                        this.image[i, k].B = 0;
                    }
                    
                }
            }
        }
        #endregion
        #region Agrandissement
        /// <summary>
        /// On agrandis selon un facteur
        /// </summary>
        /// <param name="facteur">facteur d'agrandissment</param>
        public void Agrandissement(int facteur)
        {
            if(facteur>0)
            {
                Pixel[,] Nouvelle_Image = new Pixel[this.hauteur * facteur, this.largeur * facteur];

                int indexHauteur = 0;
                int indexLargeur = 0;
                for (int i = 0; i < this.hauteur; i++)
                {
                    for (int j = 0; j < facteur; j++)
                    {
                        for (int k = 0; k < this.largeur; k++)
                        {
                            for (int l = 0; l < facteur; l++)
                            {
                                Nouvelle_Image[indexHauteur, indexLargeur] = this.image[i, k];
                                indexLargeur++;
                            }
                        }

                        indexLargeur = 0;
                        indexHauteur++;
                    }
                }
                this.image = Nouvelle_Image;
                this.hauteur = this.image.GetLength(0);
                this.largeur = this.image.GetLength(1);
            }
        }
        #endregion
        #region Fractale de Mandelbrot
        /// <summary>
        /// Réaliser une fractale de MAndelbrot
        /// </summary>
        /// <param name="c">La constante de début</param>
        /// <param name="motif">Le motif souhaité</param>
        /// <param name="niveauDetail">Le niveau de détail souhaité</param>
        /// <returns></returns>
        static byte Fractale(Point c,string motif, int niveauDetail)
        {

            //Dépendant du motif la constante de la suite de mandelbort change

            Point Z;
            if(motif=="coeur")
            {
                Z = new Point(0, 0);
            }
            else
            {
                Z = c;
            }
            Point ajout=new Point(0,0);


            switch (motif)
            {
                case "galaxie":
                    ajout.X = 0.355;
                    ajout.Y = 0.355;
                    break;
                case "vague":
                    ajout.X = -0.54;
                    ajout.Y = 0.54;
                    break;
                case "explosions":
                    ajout.X = 0.36;
                    ajout.Y = 0.1;
                    break;
                case "eclair":
                    ajout.X = 0.0;
                    ajout.Y = 0.8;
                    break;
                default:
                    ajout.X = c.X;
                    ajout.Y = c.Y;
                    break;
            }
            for (int i = 0; i < niveauDetail; i++)
            {
                Z.Carre();
                Z.X += ajout.X;
                Z.Y += ajout.Y;
                if(Z.Norme()>=2)
                {
                    return Convert.ToByte(255*(1.0 * i / niveauDetail *1.0));
                }
            }

            return 255;
        }
        /// <summary>
        /// Ecrire la matrice qui aceuillera les éléments de mandelbrot
        /// </summary>
        /// <param name="Xresolution">largeur image</param>
        /// <param name="Yresolution">longueur image</param>
        /// <param name="motif">motif souhaité</param>
        /// <param name="niveauDetail">le niveau de détail de 1 à l'infini</param>
        public void Mandelbrot(long Xresolution,long Yresolution, string motif, int niveauDetail)
        {
            this.hauteur = Yresolution;
            this.largeur = Xresolution;

            Pixel[,] NouvelleImage = new Pixel[this.hauteur, this.largeur];

            for (int i=0;i<this.hauteur;i++)
            {
                for(int j=0;j<this.largeur;j++)
                {
                    Point point = new Point((j - (this.largeur/2.0)) / (this.largeur/4.0), (i - (this.hauteur / 2.0)) / (this.hauteur / 4.0));
                    
                    byte couleur = Fractale(point, motif,niveauDetail);

                    NouvelleImage[i, j] = new Pixel(couleur, couleur, couleur);
                    
                }
            }
            this.image = NouvelleImage;
        }
        #endregion
        #region Convolution
        public void Contours2()
        {
            int[,] contour = new int[,] { { -1, -1, -1 }, { -1, 8, -1 }, { -1, -1, -1 } };
            Pixel[,] copie = new Pixel[image.GetLength(0), image.GetLength(1)];
            double valR;
            double valG;
            double valB;
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    valR = 0;
                    valG = 0;
                    valB = 0;

                    if (i == 0 || i == image.GetLength(0) - 1 || j == 0 || j == image.GetLength(1) - 1)
                    {
                        copie[i, j] = image[i, j];
                    }
                    else
                    {
                        for (int k = i - 1; k <= i + 1; k++)
                        {
                            for (int m = j - 1; m <= j + 1; m++)
                            {
                                valR += image[k, m].R * contour[k + 1 - i, m + 1 - j];
                                valG += image[k, m].G * contour[k + 1 - i, m + 1 - j];
                                valB += image[k, m].B * contour[k + 1 - i, m + 1 - j];
                            }
                        }

                        if (valR > 255)
                        {
                            valR = 255;
                        }
                        else
                        {
                            if (valR < 0)
                            {
                                valR = 0;
                            }
                        }
                        if (valG > 255)
                        {
                            valG = 255;
                        }
                        else
                        {
                            if (valG < 0)
                            {
                                valG = 0;
                            }
                        }
                        if (valB > 255)
                        {
                            valB = 255;
                        }
                        else
                        {
                            if (valB < 0)
                            {
                                valB = 0;
                            }
                        }
                        copie[i, j] = new Pixel(0,0,0);
                        copie[i, j].R = Convert.ToByte(valR);
                        copie[i, j].B = Convert.ToByte(valB);
                        copie[i, j].G = Convert.ToByte(valG);

                    }
                }
            }
            this.image = copie;
        }
        #endregion
        #region Renforcement
        public void Renforcement()
        {
            int[,] contour = new int[,] { { 0,0,0 }, { -1, 1, 0 }, { 0,0,0} };
            //int[,] contour = new int[,] { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } };
            Pixel[,] copie = new Pixel[image.GetLength(0), image.GetLength(1)];
            double valR;
            double valG;
            double valB;
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    valR = 0;
                    valG = 0;
                    valB = 0;

                    if (i == 0 || i == image.GetLength(0) - 1 || j == 0 || j == image.GetLength(1) - 1)
                    {
                        copie[i, j] = image[i, j];
                    }
                    else
                    {
                        for (int k = i - 1; k <= i + 1; k++)
                        {
                            for (int m = j - 1; m <= j + 1; m++)
                            {
                                valR += image[k, m].R * contour[k + 1 - i, m + 1 - j];
                                valG += image[k, m].G * contour[k + 1 - i, m + 1 - j];
                                valB += image[k, m].B * contour[k + 1 - i, m + 1 - j];
                            }
                        }

                        if (valR > 255)
                        {
                            valR = 255;
                        }
                        else
                        {
                            if (valR < 0)
                            {
                                valR = 0;
                            }
                        }
                        if (valG > 255)
                        {
                            valG = 255;
                        }
                        else
                        {
                            if (valG < 0)
                            {
                                valG = 0;
                            }
                        }
                        if (valB > 255)
                        {
                            valB = 255;
                        }
                        else
                        {
                            if (valB < 0)
                            {
                                valB = 0;
                            }
                        }
                        copie[i, j] = new Pixel(0, 0, 0);
                        copie[i, j].R = Convert.ToByte(valR);
                        copie[i, j].B = Convert.ToByte(valB);
                        copie[i, j].G = Convert.ToByte(valG);

                    }
                }
            }
            this.image = copie;
        }
        #endregion
        #region Flou
        public void Flou()
        {
            Pixel[,] copie = this.image;
            //double[,] flou = new double[,] { { 0.0625, 0.125, 0.0625 }, { 0.125, 0.25, 0.125 }, { 0.0625, 0.125, 0.0625 } };
            double[,] flou = new double[,] { { 0.1, 0.1, 0.1 }, { 0.1, 0.1, 0.1 }, { 0.1, 0.1, 0.1 } };
            decimal valR;
            decimal valG;
            decimal valB;
            for (int i = 1; i < copie.GetLength(0) - flou.GetLength(0) / 2; i++)
            {
                for (int j = 1; j < copie.GetLength(1) - flou.GetLength(1) / 2; j++)
                {
                    valR = 0;
                    valG = 0;
                    valB = 0;

                    for (int k = 0; k < flou.GetLength(0); k++)
                    {
                        for (int m = 0; m < flou.GetLength(1); m++)
                        {
                            valR = valR + Convert.ToDecimal(this.image[i - 1 + k, j - 1 + m].R * flou[k, m]);
                            valG = valG + Convert.ToDecimal(this.image[i - 1 + k, j - 1 + m].G * flou[k, m]);
                            valB = valB + Convert.ToDecimal(this.image[i - 1 + k, j - 1 + m].B * flou[k, m]);
                        }
                    }
                    if (valR < 0)
                        copie[i, j].R = 0;
                    else
                    {
                        if (valR > 255)
                            copie[i, j].R = 255;
                        else
                            copie[i, j].R = Convert.ToByte(valR);
                    }
                    if (valG < 0)
                        copie[i, j].G = 0;
                    else
                    {
                        if (valG > 255)
                            copie[i, j].G = 255;
                        else
                            copie[i, j].G = Convert.ToByte(valG);
                    }
                    if (valB < 0)
                        copie[i, j].B = 0;
                    else
                    {
                        if (valB > 255)
                            copie[i, j].B = 255;
                        else
                            copie[i, j].B = Convert.ToByte(valB);
                    }
                }
            }
        }
        #endregion
        #region Steganographie
        /// <summary>
        /// Ne marche pas
        /// </summary>
        /// <param name="NomImage2"></param>
        public void Steganographie(string NomImage2)
        {
            byte[] fichier2=File.ReadAllBytes(NomImage2 + ".bmp");


            byte[] tableauDecryptage = new byte[] { fichier2[10], fichier2[11], fichier2[12], fichier2[13] };
            long TailleOffset2 = Convertir_Endian_To_Int(tableauDecryptage);


            tableauDecryptage = new byte[] { fichier2[18], fichier2[19], fichier2[20], fichier2[21] };
            long largeur2 = Convertir_Endian_To_Int(tableauDecryptage);

            tableauDecryptage = new byte[] { fichier2[22], fichier2[23], fichier2[24], fichier2[25] };
            long hauteur2 = Convertir_Endian_To_Int(tableauDecryptage);
            Pixel[,] image2 = new Pixel[hauteur2, largeur2];

            long pointeur = TailleOffset2;

            for (int i = 0; i < hauteur2; i++)
            {
                for (int j = 0; j < largeur2; j++)
                {
                    image2[i, j] = new Pixel(0, 0, 0);
                    for (int k = 0; k < 3; k++)
                    {
                        if (k == 0)
                            image2[i, j].R = fichier2[pointeur];
                        else if (k == 1)
                            image2[i, j].G = fichier2[pointeur];
                        else if (k == 2)
                            image2[i, j].B = fichier2[pointeur];
                        pointeur++;
                    }
                }

            }
            long hauteurMin = 0;
            long largeurMin = 0;

            if (this.hauteur > hauteur2)
            {
                hauteurMin = hauteur2;
            }
            else
            {
                hauteurMin = this.hauteur;
            }

            if (this.largeur > largeur2)
            {
                largeurMin = largeur2;
            }
            else
            {
                largeurMin = this.largeur;
            }

            Console.WriteLine(hauteur + " " + hauteur2 + " " + hauteurMin);
            Console.WriteLine(largeur + " " + largeur2 + " " + largeurMin);


            Pixel[,] NouvelleImage = new Pixel[hauteurMin, largeurMin];

            for (int i = 0; i < hauteurMin; i++)
            {
                for (int j = 0; j < largeurMin; j++)
                {
                    NouvelleImage[i, j] = new Pixel(0, 0, 0);

                    //Rouge
                    int[] Couleur1 = Convertir_byte_to_binary(this.image[i,j].R,8);
                    int[] Couleur2 = Convertir_byte_to_binary(image2[i, j].R, 8);

                    int[] CouleurFinale = new int[8] { Couleur1[0], Couleur1[1], Couleur1[2], Couleur1[3], Couleur2[0], Couleur2[1], Couleur2[2], Couleur2[3] };

                    byte RougeFinal = Convertir_Binary_To_Byte(CouleurFinale);
                    NouvelleImage[i,j].R=RougeFinal;



                    //Vert
                    Couleur1 = Convertir_byte_to_binary(this.image[i, j].G, 8);
                    Couleur2 = Convertir_byte_to_binary(image2[i, j].G, 8);

                    CouleurFinale = new int[8] { Couleur1[0], Couleur1[1], Couleur1[2], Couleur1[3], Couleur2[0], Couleur2[1], Couleur2[2], Couleur2[3] };

                    byte VertFinal = Convertir_Binary_To_Byte(CouleurFinale);
                    NouvelleImage[i, j].G = VertFinal;




                    //Bleu
                    Couleur1 = Convertir_byte_to_binary(this.image[i, j].B, 8);
                    Couleur2 = Convertir_byte_to_binary(image2[i, j].B, 8);

                    CouleurFinale = new int[8] { Couleur1[0], Couleur1[1], Couleur1[2], Couleur1[3], Couleur2[0], Couleur2[1], Couleur2[2], Couleur2[3] };

                    byte BleuFinal = Convertir_Binary_To_Byte(CouleurFinale);
                    NouvelleImage[i, j].B = BleuFinal;


                }
            }
            this.image = NouvelleImage;
            this.hauteur = NouvelleImage.GetLength(0);
            this.largeur = NouvelleImage.GetLength(1);
        }
        #endregion
        #region Desteganographie
        /// <summary>
        /// Ne marche pas
        /// </summary>
        /// <param name="fichier1"></param>
        /// <param name="fichier2"></param>
        public void Desteganographie(string fichier1, string fichier2)
        {
            Pixel[,] image1 = new Pixel[this.hauteur, this.largeur];
            Pixel[,] image2 = new Pixel[this.hauteur, this.largeur];
            for(int i=0;i<this.hauteur; i++)
            {
                for(int j=0;j<this.largeur; j++)
                {
                    image1[i, j] = new Pixel(0, 0, 0);
                    image2[i, j] = new Pixel(0, 0, 0);

                    //Rouge

                    int[] CouleurFab = Convertir_byte_to_binary(image[i, j].R,8);
                    int[] Couleur1 = new int[4] { CouleurFab[0], CouleurFab[1], CouleurFab[2], CouleurFab[3] };
                    int[] Couleur2 = new int[4] { CouleurFab[4], CouleurFab[5], CouleurFab[6], CouleurFab[7] };

                    image1[i, j].R = Convertir_Binary_To_Byte(Couleur1);
                    image2[i,j].R=Convertir_Binary_To_Byte(Couleur2);

                    //Vert

                    CouleurFab = Convertir_byte_to_binary(image[i, j].G, 8);
                    Couleur1 = new int[4] { CouleurFab[0], CouleurFab[1], CouleurFab[2], CouleurFab[3] };
                    Couleur2 = new int[4] { CouleurFab[4], CouleurFab[5], CouleurFab[6], CouleurFab[7] };

                    image1[i, j].G = Convertir_Binary_To_Byte(Couleur1);
                    image2[i, j].G = Convertir_Binary_To_Byte(Couleur2);

                    //Bleu

                    CouleurFab = Convertir_byte_to_binary(image[i, j].B, 8);
                    Couleur1 = new int[4] { CouleurFab[0], CouleurFab[1], CouleurFab[2], CouleurFab[3] };
                    Couleur2 = new int[4] { CouleurFab[4], CouleurFab[5], CouleurFab[6], CouleurFab[7] };

                    image1[i, j].B = Convertir_Binary_To_Byte(Couleur1);
                    image2[i, j].B = Convertir_Binary_To_Byte(Couleur2);
                }
            }
            Pixel[,] memoire = this.image;

            this.image = image1;
            #region Enregistrement1
            this.hauteur = this.image.GetLength(0);
            this.largeur = this.image.GetLength(1);
            long index = 0;
            byte[] FichierBMP = new byte[3 * (this.hauteur * this.largeur) + 54];
            this.TailleFile = FichierBMP.Length;
            #region Header
            for (int i = 0; i < 2; i++)
            {
                FichierBMP[index] = Convert.ToByte(this.type[i]);
                index++;
            }



            byte[] FileSize = Convertir_Int_To_Endian(this.TailleFile, 4);
            for (int i = 0; i < FileSize.Length; i++)
            {
                FichierBMP[index] = FileSize[i];
                index++;
            }



            for (int i = 0; i < 4; i++)
            {
                FichierBMP[index] = 0;
                index++;
            }


            byte[] FileOffset = Convertir_Int_To_Endian(this.TailleOffset, 4);
            for (int i = 0; i < FileOffset.Length; i++)
            {
                FichierBMP[index] = FileOffset[i];
                index++;
            }



            #endregion
            #region InfoHeader

            byte[] tailleInfoHeader = Convertir_Int_To_Endian(this.tailleInfoHeader, 4);
            for (int i = 0; i < tailleInfoHeader.Length; i++)
            {
                FichierBMP[index] = tailleInfoHeader[i];
                index++;
            }


            byte[] largeur = Convertir_Int_To_Endian(this.largeur, 4);
            for (int i = 0; i < largeur.Length; i++)
            {
                FichierBMP[index] = largeur[i];
                index++;
            }


            byte[] hauteur = Convertir_Int_To_Endian(this.hauteur, 4);
            for (int i = 0; i < hauteur.Length; i++)
            {
                FichierBMP[index] = hauteur[i];
                index++;
            }


            byte[] planes = Convertir_Int_To_Endian(this.planes, 2);
            for (int i = 0; i < planes.Length; i++)
            {
                FichierBMP[index] = planes[i];
                index++;
            }


            byte[] BitsParCouleur = Convertir_Int_To_Endian(this.BitsParCouleur, 2);
            for (int i = 0; i < BitsParCouleur.Length; i++)
            {
                FichierBMP[index] = BitsParCouleur[i];
                index++;
            }


            byte[] Compression = Convertir_Int_To_Endian(this.compression, 4);
            for (int i = 0; i < Compression.Length; i++)
            {
                FichierBMP[index] = Compression[i];
                index++;
            }


            byte[] ImageSize = Convertir_Int_To_Endian(this.imageSize, 4);
            for (int i = 0; i < ImageSize.Length; i++)
            {
                FichierBMP[index] = ImageSize[i];
                index++;
            }


            for (int i = 0; i < 16; i++)
            {
                FichierBMP[index] = 0;
                index++;
            }

            #endregion
            for (int i = 0; i < this.hauteur; i++)
            {
                for (int k = 0; k < this.largeur; k++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (j == 0)
                            FichierBMP[index] = image[i, k].R;
                        else if (j == 1)
                            FichierBMP[index] = image[i, k].G;
                        else if (j == 2)
                            FichierBMP[index] = image[i, k].B;

                        index++;
                    }
                }
            }
            /*for(int i=0; i < 54; i++)
            {
                Console.WriteLine(FichierBMP[i]);
            }
            int compte = 0;
            for (int i = 54; i < FichierBMP.Length; i++)
            {
                if ( i%25==0)
                {
                    Console.WriteLine();
                }
                if (i % 3 == 0)
                {
                    Console.Write("|");
                    compte++;
                }

                Console.Write(FichierBMP[i] + " ");
                
            }
            Console.WriteLine(compte);*/
            File.WriteAllBytes(fichier1 + ".bmp", FichierBMP);
            #endregion

            this.image = image2;
            #region Enregistrement2
            this.hauteur = this.image.GetLength(0);
            this.largeur = this.image.GetLength(1);
            index = 0;
            FichierBMP = new byte[3 * (this.hauteur * this.largeur) + 54];
            this.TailleFile = FichierBMP.Length;
            #region Header
            for (int i = 0; i < 2; i++)
            {
                FichierBMP[index] = Convert.ToByte(this.type[i]);
                index++;
            }



            FileSize = Convertir_Int_To_Endian(this.TailleFile, 4);
            for (int i = 0; i < FileSize.Length; i++)
            {
                FichierBMP[index] = FileSize[i];
                index++;
            }



            for (int i = 0; i < 4; i++)
            {
                FichierBMP[index] = 0;
                index++;
            }


            FileOffset = Convertir_Int_To_Endian(this.TailleOffset, 4);
            for (int i = 0; i < FileOffset.Length; i++)
            {
                FichierBMP[index] = FileOffset[i];
                index++;
            }



            #endregion
            #region InfoHeader

            tailleInfoHeader = Convertir_Int_To_Endian(this.tailleInfoHeader, 4);
            for (int i = 0; i < tailleInfoHeader.Length; i++)
            {
                FichierBMP[index] = tailleInfoHeader[i];
                index++;
            }


            largeur = Convertir_Int_To_Endian(this.largeur, 4);
            for (int i = 0; i < largeur.Length; i++)
            {
                FichierBMP[index] = largeur[i];
                index++;
            }


            hauteur = Convertir_Int_To_Endian(this.hauteur, 4);
            for (int i = 0; i < hauteur.Length; i++)
            {
                FichierBMP[index] = hauteur[i];
                index++;
            }


            planes = Convertir_Int_To_Endian(this.planes, 2);
            for (int i = 0; i < planes.Length; i++)
            {
                FichierBMP[index] = planes[i];
                index++;
            }


            BitsParCouleur = Convertir_Int_To_Endian(this.BitsParCouleur, 2);
            for (int i = 0; i < BitsParCouleur.Length; i++)
            {
                FichierBMP[index] = BitsParCouleur[i];
                index++;
            }


            Compression = Convertir_Int_To_Endian(this.compression, 4);
            for (int i = 0; i < Compression.Length; i++)
            {
                FichierBMP[index] = Compression[i];
                index++;
            }


            ImageSize = Convertir_Int_To_Endian(this.imageSize, 4);
            for (int i = 0; i < ImageSize.Length; i++)
            {
                FichierBMP[index] = ImageSize[i];
                index++;
            }


            for (int i = 0; i < 16; i++)
            {
                FichierBMP[index] = 0;
                index++;
            }

            #endregion
            for (int i = 0; i < this.hauteur; i++)
            {
                for (int k = 0; k < this.largeur; k++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (j == 0)
                            FichierBMP[index] = image[i, k].R;
                        else if (j == 1)
                            FichierBMP[index] = image[i, k].G;
                        else if (j == 2)
                            FichierBMP[index] = image[i, k].B;

                        index++;
                    }
                }
            }
            File.WriteAllBytes(fichier2 + ".bmp", FichierBMP);
            #endregion

        }
        #endregion
        #region Huffmann
        /// <summary>
        /// Crée un tableau avec le code de huffman
        /// </summary>
        /// <returns>le tableau avec, les bytes, la fréquence, et l'encodage en binaire</returns>
        public List<string[]> HuffMan()
        {
            //pour simplifier le code on met l'image au niveau de gris
            Pixel[,] imageGris=new Pixel[this.hauteur,this.largeur];

            for (int i = 0; i < this.hauteur; i++)
            {
                for (int k = 0; k < this.largeur; k++)
                {
                    imageGris[i, k] = new Pixel(0, 0, 0);
                    byte moyenne = this.image[i, k].moyenne();
                    imageGris[i, k].R = moyenne;
                    imageGris[i, k].G = moyenne;
                    imageGris[i, k].B = moyenne;
                }
            }
            List<string[]> TabHuffman = new List<string[]>();

            for (int i = 0; i < this.hauteur; i++)
            {
                for (int j = 0; j < this.largeur; j++)
                {
                    int index = TestPresence(TabHuffman, "" + imageGris[i, j].R);
                    if (index==-1)
                    {
                        TabHuffman.Add(new string[3] { "" + imageGris[i, j].R, "1","0" });
                    }
                    else
                    {
                        TabHuffman[index][1] = "" + (Convert.ToInt32(TabHuffman[index][1]) + 1);
                    }
                }
            }
            for(int i=0;i<TabHuffman.Count;i++)
            {
                TabHuffman[i][1] = "" + (Convert.ToInt32(TabHuffman[i][1])*1.0 / (this.hauteur*this.largeur)*1.0);
            }


            for (int k = 0; k < TabHuffman.Count; k++)
            {
                for (int i = 0; i < TabHuffman.Count-1; i++)
                {
                    if(Convert.ToDouble(TabHuffman[i][1]) < Convert.ToDouble(TabHuffman[i+1][1]))
                    {
                        string[] memoire = TabHuffman[i];
                        TabHuffman[i] = TabHuffman[i + 1];
                        TabHuffman[i + 1] = memoire;
                    }
                }
            }

            for (int i = 0; i < TabHuffman.Count; i++)
            {
                for (int k = 0; k < i; k++)
                    TabHuffman[i][2] = "1" + TabHuffman[i][2];
                if(i==TabHuffman.Count-1)
                    TabHuffman[i][2] = TabHuffman[i][2].Substring(0, TabHuffman[i][2].Length-1);
            }
            return TabHuffman;
        }
        #endregion
        #region Verification si élément dans le tableau
        /// <summary>
        /// On regarde si un élément byte est dans le tableau
        /// </summary>
        /// <param name="tab">le tableau en question</param>
        /// <param name="element">l'élément byte à chercher</param>
        /// <returns></returns>
        public static int TestPresence(List<string[]> tab, string element)
        {
            int index =-1;
            if (tab != null && tab.Count != 0)
            {
                for (int i = 0; i < tab.Count && index==-1; i++)
                {
                    if (tab[i][0] == element)
                        index = i;
                }
            }
            return index;
        }
        #endregion
    }
}