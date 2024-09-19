using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ProblemeInfo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu();
            /*string fichier = "coco";
            //Process.Start(fichier + ".bmp");
            
            MyImage image = new MyImage(fichier,true);
            List<string[]> tab=image.HuffMan();
            for (int i = 0; i < tab.Count; i++)
            {
                Console.WriteLine(tab[i][0] + " " + tab[i][1]+" " + tab[i][2]);

            }
            Console.ReadLine();*/
        }
        #region Menu:
        static void Menu()
        {
            Console.Clear(); // réinitialise la console

            try
            {
                Console.WriteLine(File.ReadAllText("titre.txt"));
            }
            catch(IOException e)
            {
                Console.WriteLine("Une erreur s'est produite lors de la lecture du fichier : " + e.Message);
            }
            Console.WriteLine("Bonjour, bienvenue sur notre logiciel de traitement d'image" +
                "\nSouhaitez vous changer une image déjà existante(1) ou en créer une (2)?" +
                "\nAppuyez sur escape pour sortir");
            int choix = 0;
            while(choix !=1 && choix !=2 && choix!=-10)
            {
                choix=SaisieNombre();
            }
            #region EditionImage
            if (choix ==1)
            {
                Console.Clear();
                Console.WriteLine("Quel est le nom de cette image ? (coco/lena/lac/test)");
                string nomImage = Console.ReadLine();
                MyImage image = new MyImage(nomImage, true);
                Process.Start(nomImage + ".bmp");
                Console.Clear();
                Console.WriteLine("Voilà ce qu'on peut faire à l'image:\n" +
                " 1. Quart supérieur gauche \n" +
                " 2. Niveau de gris \n" +
                " 3. Rotation (attention plusieurs rotations cassent l'image, pensez à n'en faire qu'une et en dernier\n" +
                " 4. Noir et blanc\n" +
                " 5. Agrandissement\n" +
                " 6. Détection des bords\n" +
                " 7. Renforcement des bords\n" +
                " 8. Flou\n" +
                "Escape: Sortir\n");
                while (choix!=-10)
                {
                    choix = -1;
                    choix = SaisieNombre();
                    while (choix != 1 && choix != 2 && choix != 3 && choix != 4 && choix != 5 && choix != 6 && choix != 7 && choix != 8 && choix != -10)
                        choix = SaisieNombre();

                    switch (choix)
                    {
                        case -10:
                            break;
                        case 1:
                            image.Quart();
                            Console.WriteLine("Un quart a été effectué");
                            break;
                        case 2:
                            image.Niveau_Gris();
                            Console.WriteLine("Un niveau de gris a été effectué");
                            break;
                        case 3:
                            Console.WriteLine("Quel angle (en degré)?");
                            image.Rotation(Convert.ToDouble(Console.ReadLine()));
                            Console.WriteLine("Une rotation a été effectuée");
                            break;
                        case 4:
                            image.Noir_et_Blanc();
                            Console.WriteLine("Un noir et blanc a été effectué");
                            break;
                        case 5:
                            int facteur = -1;
                            Console.WriteLine("Quel facteur d'agrandissement ?");
                            while(facteur <= 0)
                            {
                                facteur = Convert.ToInt32(Console.ReadLine());
                            }
                            image.Agrandissement(facteur);
                            Console.WriteLine("Un agrandissement a été effectué");
                            break;
                        case 6:
                            image.Contours2();
                            Console.WriteLine("Un contour a été effectué");
                            break;
                        case 7:
                            image.Renforcement();
                            Console.WriteLine("Un renforcement a été effectué");
                            break;
                        case 8:
                            image.Flou();
                            Console.WriteLine("Un floutage a été effectué");
                            break;
                        default:
                            break;
                    }
                }
                Fin(image);
            }
            #endregion
            #region Fractale
            else if(choix==2)
            {
                Console.Clear();
                MyImage image = new MyImage("", false);
                Console.WriteLine("Vous avez dit avoir souhaité créer une image nous disposons de la création de fractale");
                Console.WriteLine("Que souhaitez vous comme motif de fractale:" +
                    "\n 1. Le motif classique de Mandelbrot" +
                    "\n 2. Des vagues" +
                    "\n 3. Des galaxie" +
                    "\n 4. Des explosions" +
                    "\n 5. Un éclair ");
                choix = 0;
                string motif;
                while (choix != 1 && choix != 2 && choix != 3 && choix != 4 && choix != 5)
                {
                    choix = SaisieNombre();
                }
                switch (choix)
                {
                    case 1:
                        motif = "coeur";
                        break;
                    case 2:
                        motif = "vague";
                        break;
                    case 3:
                        motif = "galaxie";
                        break;
                    case 4:
                        motif = "explosions";
                        break;
                    case 5:
                        motif = "eclair";
                        break;
                    default:
                        motif = "coeur";
                        break;
                }

                long hauteur = -1;
                long largeur = -1;
                Console.WriteLine("Quelle hauteur en pixels ?");
                while (hauteur < 0)
                    hauteur = Convert.ToInt64(Console.ReadLine());
                Console.WriteLine("Quelle largeur en pixels ?");
                while (largeur < 0)
                    largeur = Convert.ToInt64(Console.ReadLine());

                int detail = -1;
                Console.WriteLine("Quel niveau de détail ?");
                while (detail < 0)
                    detail = Convert.ToInt32(Console.ReadLine());
                image.Mandelbrot(largeur, hauteur, motif, detail);
                Fin(image);
            }else if(choix == -10)
            {

            }
            #endregion            
        }
        #endregion
        #region Fin:
        static void Fin(MyImage image)
        {
            string nomImage;
            Console.WriteLine(" Sous quel nom souhaitez vous enregistrer votre image");
            nomImage = Console.ReadLine();
            image.From_Image_To_File(nomImage);
            Process.Start(nomImage+".bmp");


            Console.WriteLine("\nTapez Escape pour sortir ou sur une autre touche pour revenir au menu");
            
            if (SaisieNombre() != -10)
                Menu();
        }
        #endregion
        #region Saisie nombre:
        static int SaisieNombre()
        {
            ConsoleKeyInfo cki; //déclare une variable de type ConsoleKeyInfo 
            cki = Console.ReadKey(); // cki contient entre autres le code de la touche sur laquelle l’utilisateur a appuyé
            int nombre=-1;
            switch (cki.Key)
            {
                case ConsoleKey.D1:
                    nombre = 1;
                    break;
                case ConsoleKey.D2:
                    nombre = 2;
                    break;
                case ConsoleKey.D3:
                    nombre = 3;
                    break;
                case ConsoleKey.D4:
                    nombre = 4;
                    break;
                case ConsoleKey.D5:
                    nombre = 5;
                    break;
                case ConsoleKey.D6:
                    nombre = 6;
                    break;
                case ConsoleKey.D7:
                    nombre = 7;
                    break;
                case ConsoleKey.D8:
                    nombre = 8;
                    break;
                case ConsoleKey.Escape:
                    nombre = -10;
                    break;
            }
            return nombre;
        }
        #endregion
    }
}
