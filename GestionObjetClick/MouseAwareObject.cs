using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommunXnaFree.Spacialisation;
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
        /// Graphique device, sert a colorer.
        /// </summary>
        private GraphicsDevice gd;


        /// <summary>
        /// Proto
        /// </summary>
       public bool isMovable = false;
        #endregion

        #region accesseurs

        public virtual Texture2D SpriteToDraw { get; set; }

        public virtual Coordonnees PositionToDraw
        {
            get
            {
                return Position;
            }
        
        }
        public  Coordonnees Position { get; set; }

        public Color Couleur { get; set; }

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
        /// <param name="g"></param>
        /// <param name="text">Nom de l'objet</param>
        /// <param name="z">Position Z, plus Z est faible, plus il sera devant</param>
        /// <param name="gd">GRaphiqueDevice</param>
        /// <param name="id"></param>
        /// <param name="c">Colorkey</param>
        /// <param name="pos">Position de son sprite</param>
        public MouseAwareObject(Game g,  float z, GraphicsDevice gd, string id = "NoID", Coordonnees pos = default(Coordonnees)) : base(g)
        {
            this.gd = gd;
            Position = pos;
            Z = z;
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

        public void ColorerCurrentTexture( int alphamin)
    {
        this.Sprite_coloree = Colorer(this.SpriteToDraw, Couleur, alphamin, gd);
    }


        public override void Draw(GameTime gameTime)
        {
            //var sp = new SpriteBatch(Game.GraphicsDevice);
            //sp.Begin();
            //sp.Draw(this.SpriteToDraw, new Rectangle((int)this.Position.X, (int)this.Position.Y, this.SpriteToDraw.Width, this.SpriteToDraw.Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, this.Z);
            //sp.End();
           
            //base.Draw(gameTime);

        }

        public void SaveColoreeAsPng(string path)
        {
            using (var temp = File.Open(path, FileMode.Create))
                this.Sprite_coloree.SaveAsPng(temp, Sprite_coloree.Width, Sprite_coloree.Height);
        }

        #endregion
    }
}
