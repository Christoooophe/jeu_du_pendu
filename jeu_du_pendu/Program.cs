using System;
using AsciiArt;

namespace jeu_du_pendu
{
    internal class Program
    {
        static void AfficherMot(string mot, List<char> lettres)
        {
            for (int i = 0; i < mot.Length; i++)
            {
                char lettre = mot[i];
                if(lettres.Contains(lettre))
                {
                    Console.Write(lettre + " ");
                }
                else
                {
                    Console.Write("_ ");
                }
            }
            Console.WriteLine();
        }

        static bool ToutesLettresDevines(string mot, List<char> lettres) 
        { 
            foreach(var lettre in lettres)
            {
                mot = mot.Replace(lettre.ToString(), "");
            }
            if (mot.Length == 0)
            {
                return true;
            }
            return false;
        }
        static char DemanderUneLettre(string message = "Rentrez une lettre : ")
        {
            while (true)
            {
                Console.Write(message);
                string reponse = Console.ReadLine();
                if (reponse.Length == 1)
                {
                    reponse = reponse.ToUpper();
                    return reponse[0];
                }
                Console.WriteLine("ERREUR : On a dit de rentrer une lettre");
                Console.WriteLine();

            }


        }
        static void DevinerMot(string mot)
        {
            var liste = new List<char>();
            var listeLettresPasDansLeMot = new List<char>();
            const int NB_VIES = 6;
            int viesRestants = NB_VIES;

            while (viesRestants > 0)
            {
                AfficherMot(mot, liste);

                Console.WriteLine("Nombre de vies restantes : "+viesRestants);
                Console.WriteLine();
                var lettreUtilisateur = DemanderUneLettre();
                Console.Clear();
                if (mot.Contains(lettreUtilisateur))
                {
                    liste.Add(lettreUtilisateur);
                    if(ToutesLettresDevines(mot, liste))
                    {
                        Console.WriteLine("GG !");
                        return;
                    }
                }
                else
                {
                    if (!listeLettresPasDansLeMot.Contains(lettreUtilisateur)){
                        listeLettresPasDansLeMot.Add(lettreUtilisateur);
                        viesRestants--;
                    }
                }
                if(listeLettresPasDansLeMot.Count> 0)
                {
                    Console.WriteLine("Le mot ne contient pas les lettres : " + String.Join(", ", listeLettresPasDansLeMot));
                    Console.WriteLine();
                }
            Console.WriteLine(Ascii.PENDU[NB_VIES - viesRestants]);

            }

            if (viesRestants== 0)
            {
                Console.WriteLine("Perdu ! Le mot était : " +mot);
            }

        }

        static string[] ChargerLesMots(string nomFichier)
        {
            try
            {
                return File.ReadAllLines(nomFichier);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Erreur de lecture du fichier" + nomFichier + " (" +ex.Message + ")");
            }

            return null;
        }

        static bool DemanderDeRejouer()
        {
            char reponse = DemanderUneLettre("Voulez vous rejouer ? O/N");
            if((reponse == 'o') || (reponse == 'O'))
            {
                return true;
            }
            else if ((reponse == 'n') || (reponse == 'N'))
            {
                return false;
            }
            else
            {
                Console.WriteLine("Erreur");
                return DemanderDeRejouer();
            }
        }
        static void Main(string[] args)
        {
            var rand = new Random();
            var mots = ChargerLesMots("mots.txt");

            if((mots == null ) || (mots.Length == 0)) {
                Console.WriteLine("La liste de mots est vide");
            }
            else
            {
                while(true )
                {
                    string mot = mots[rand.Next(mots.Length)].Trim().ToUpper();
                    DevinerMot(mot);
                    if (!DemanderDeRejouer())
                    {
                        break;
                    }
                    Console.Clear();
                }
                
            }


        }
    }
}
