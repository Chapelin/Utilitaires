using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace GestionObjetClick
{
    /// <summary>
    /// Classe de gestion des objets
    /// </summary>
    public class MouseAwareObjectManager
    {

        #region Variables de classe
        /// <summary>
        /// Dictionnaire des objets sous la forme nom,objet
        /// </summary>
        Dictionary<Color,MouseAwareObject> dic;


        /// <summary>
        /// Game en cours
        /// </summary>
        Game game;

        /// <summary>
        /// Texture cachee contenant les sprites colorées
        /// </summary>
        Texture2D cachee;

        /// <summary>
        /// RenderTarget servant a cachee
        /// </summary>
        RenderTarget2D renderTarget;

        /// <summary>
        /// Couleur actuelle
        /// </summary>
        uint actualcolor;
        #endregion


        #region Constructeurs
        /// <summary>
        /// Constructeur d'OBjetManager
        /// </summary>
        /// <param name="g">Game</param>
        public MouseAwareObjectManager(Game g)
        {
            dic = new Dictionary<Color, MouseAwareObject>();
            this.game = g;
           
            cachee = null;

            actualcolor = 0xA00001;

        }

        #endregion


        #region Gestion des listes d'objets


        /// <summary>
        /// Ajout d'un objet au manager
        /// </summary>
        /// <param name="ob">MouseAwareObject a ajouter</param>
        public void Add( MouseAwareObject ob)
        {

            Color t = new Color((byte)(actualcolor >> 16), (byte)(actualcolor >> 8), (byte)(actualcolor));
            ob.Couleur = t;
            dic.Add(ob.Couleur, ob);
            ob.Game.Components.Add(ob);
            actualcolor= actualcolor+1;
        }

        /// <summary>
        /// Recuperation d'un objet en fonction de son nom
        /// </summary>
        /// <param name="s">Nom de l'objet</param>
        /// <returns>L'objet identifié, ou null s'il n'existe pas</returns>
        public MouseAwareObject GetObjet(String s)
        {
            MouseAwareObject temp = null;
            try
            {
                temp = dic.First(x=> x.Value.Id == s).Value;
            }
            catch { }
            
            return temp;
        }

        /// <summary>
        /// Recuperation du nom de l'objet par sa couleur
        /// </summary>
        /// <param name="c">couleur de l'objet</param>
        /// <returns>le nom de l'objet, ou null</returns>
        public String GetName(Color c)
        {
            String temp = null;
            try
            {
                temp = dic[c].Id;
            }
            catch { };

            return temp;
        }

        /// <summary>
        /// Supprime un objet du manager
        /// </summary>
        /// <param name="s"></param>
        public void Remove(String s)
        {
            try
            {
                var ob = GetObjet(s);
                dic.Remove(ob.Couleur);
                ob.Game.Components.Remove(ob);
            }
            catch { }
            
        }

        /// <summary>
        /// Mise a Zero du manager
        /// </summary>
        public void Clear()
        {
            dic = new Dictionary<Color, MouseAwareObject>();
        }


        /// <summary>
        /// Retourne la liste des objets
        /// </summary>
        /// <returns>Liste des objets triés par leur Z dans l'ordre croissant</returns>
        public List<MouseAwareObject> ToList()
        {
            List<MouseAwareObject> ltemp = new List<MouseAwareObject>();
            try
            {
                dic.OrderBy(temp => temp.Value.Z);
                ltemp = dic.Select(temp => temp.Value).ToList();
            }
            catch { }
            return ltemp;
        }

        #endregion


        #region Méthodes de dessin, caché ou non

        /// <summary>
        /// Dessine dans cachee les sprties colorées
        /// </summary>
        public void DrawHiddenObjets()
        {
            //return;
            // Set the render target
            List<MouseAwareObject> l = this.ToList();
            var renderTarget = new RenderTarget2D(
                game.GraphicsDevice,
                game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                game.GraphicsDevice.PresentationParameters.BackBufferHeight,
                false, SurfaceFormat.Color, DepthFormat.Depth24, 2, RenderTargetUsage.PreserveContents
                );
            game.GraphicsDevice.SetRenderTarget(renderTarget);
            SpriteBatch te = new SpriteBatch(game.GraphicsDevice);
            te.Begin();

            for (int i = 0; i < l.Count; i++)
            {
                l[i].ColorerCurrentTexture(100);
                te.Draw(l[i].Sprite_coloree, new Vector2(l[i].Position.X, l[i].Position.Y), Color.White);
            }
            te.End();
            // Drop the render target
            
            cachee = renderTarget;
            game.GraphicsDevice.SetRenderTarget(null);
            //cachee = renderTarget.GetTexture<Texture2D>(cachee);
        }

        //public void Draw(GameTime gameTime)
        //{
            
        //    var sp = new SpriteBatch(game.GraphicsDevice);
        //    sp.Begin();
        //    DrawHiddenObjets();
        //    sp.End();
        //}
        #endregion


        #region Utilitaires

        /// <summary>
        /// Retourne le nom de l'objet sous la souris
        /// </summary>
        /// <param name="state">MouseState</param>
        /// <returns>Nom de l'objet, ou Empty</returns>
        public String DevinerNomObjet(MouseState state)
        {
            Color[] dessous = new Color[1];
            string retour = String.Empty;
            if (state.X > 0 && state.Y > 0 && state.X < game.GraphicsDevice.PresentationParameters.BackBufferWidth && state.Y < game.GraphicsDevice.PresentationParameters.BackBufferHeight)
            {
                cachee.GetData<Color>(0, new Rectangle(state.X, state.Y, 1, 1), dessous, 0, 1);
                if (GetName(dessous[0]) != null)
                {
                    retour = this.GetName(dessous[0]);
                }
                else
                {
                    retour = String.Empty;
                }
            }
            else
                retour = String.Empty;
            
            return retour;
        }

        public MouseAwareObject DevinerObject(MouseState state)
        {
            Color[] dessous = new Color[1];
            string retour = String.Empty;
            if (state.X > 0 && state.Y > 0 && state.X < game.GraphicsDevice.PresentationParameters.BackBufferWidth && state.Y < game.GraphicsDevice.PresentationParameters.BackBufferHeight)
            {
                cachee.GetData<Color>(0, new Rectangle(state.X, state.Y, 1, 1), dessous, 0, 1);
                if (GetName(dessous[0]) != null)
                {
                    retour = this.GetName(dessous[0]);
                }
                else
                {
                    retour = String.Empty;
                }
            }
            else
                retour = String.Empty;

            return GetObjet(retour);
        }

        /// <summary>
        /// Prend un screen en png
        /// </summary>
        /// <param name="path">Chemine et non du ficher</param>
        public void TakeScreenOfHidden(String path)
        {
            using(var temp = File.Open(path,FileMode.Create))
                cachee.SaveAsPng(temp, cachee.Width, cachee.Height);
        }

        #endregion
    }
}
