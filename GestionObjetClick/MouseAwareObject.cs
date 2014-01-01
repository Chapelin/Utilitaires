using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GestionObjetClick
{
    /// <summary>
    /// Classe d'objet survolable a la souris.
    /// </summary>
    public class MouseAwareObject : DrawableGameComponent
    {


        #region variable const static
        /// <summary>
        /// Valeur d'alpha minimum pour la reconnaissance des pixels
        /// </summary>
        public const int ALPHAMIN = 100; 

        #endregion

        #region Variables de classe

        /// <summary>
        /// Position Z. Plus z est petot, plus le sprite sera devant pour la reconnaissance.
        /// Compris entre 0 et 1
        /// </summary>
        private float z;

        /// <summary>
        /// Couleur de sprite_coloree
        /// </summary>
        private Color couleur;

        /// <summary>
        /// Graphique device, sert a colorer.
        /// </summary>
        private GraphicsDevice gd;


        /// <summary>
        /// Proto
        /// </summary>
       public bool isMovable = false;
        #endregion

        #region accesseurs

        public Texture2D Sprite { get; set; }

        public Vector2 Position { get; set; }

        public Color Couleur
        {
            get { return couleur; }
            set { 
                this.Sprite_coloree = Colorer(this.Sprite,value,ALPHAMIN,this.gd);
                couleur= value; }
        }

        public float Z
        {
            get { return z; }
            set
            {
                if (value > 1)
                    z = 1;
                else
                    if (value < 0)
                        z = 0;
                    else
                        z = value;
            }
        }


        public Texture2D Sprite_coloree { get; set; }

        /// <summary>
        /// Id de l'objet : un guid, un nom, etc
        /// </summary>
        public string Id { get; set; }

        #endregion

        #region constructeur

        /// <summary>
        /// Constructeur d'objet
        /// </summary>
        /// <param name="text">Nom de l'objet</param>
        /// <param name="pos">Position de son sprite</param>
        /// <param name="z">Position Z, plus Z est faible, plus il sera devant</param>
        /// <param name="gd">GRaphiqueDevice</param>
        /// <param name="c">Colorkey</param>
        public MouseAwareObject(Game g,Texture2D text, Vector2 pos, float z, GraphicsDevice gd, string id = "NoID", Color c = default(Color)) : base(g)
        {
            this.gd = gd;
            Sprite = text;
            Position = pos;
            Z = z;
            Couleur = c;
            this.Id = id;

        }

     

        #endregion

        #region private methodes
        /// <summary>
        /// Retourne une texture colorée pixel par pixel
        /// </summary>
        /// <param name="image">texture a colorer</param>
        /// <param name="couleur">Couleur a appliquer</param>
        /// <param name="alphamin">Alpha a partir duquel les pixels sont colorés</param>
        /// <param name="gd">Graphiqudevice</param>
        /// <returns>Texture colorée</returns>
        private Texture2D Colorer(Texture2D image, Color couleur, int alphamin, GraphicsDevice gd)
        {
            Color[] retrievedColor = new Color[image.Height * image.Width];//creation ud tableau de couleurs
            image.GetData<Color>(retrievedColor); //recuperation des couleurs dans ce tableau
            Texture2D temp = new Texture2D(gd, image.Width, image.Height); //creation de la nouvelle texture, indispensable

            for (int i = 0; i < retrievedColor.Length; i++) //pour chaque px
            {
                if (retrievedColor[i].A > alphamin) //si alpha > limite
                {
                    retrievedColor[i] = couleur; //colorisation
                }
            }
            temp.SetData<Color>(retrievedColor);//enregistrement de la version colorée dans la texture tempo
            return temp;

        }

        public override void Draw(GameTime gameTime)
        {
            var sp = new SpriteBatch(Game.GraphicsDevice);
            sp.Begin();
            sp.Draw(this.Sprite, new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Sprite.Width, this.Sprite.Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, this.Z);
            sp.End();
           
            base.Draw(gameTime);
        }

        #endregion
    }
}
