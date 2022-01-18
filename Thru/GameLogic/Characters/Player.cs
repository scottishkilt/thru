﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Thru
{
	public class Player : ICharacter
	{
		public Stats stats;
		public string Name;
		public string TrailName;
		public DateTime Date;
		public string locationID;
		public string Perk;
		public Location Location, TrailLocation;
		public CharacterModel CharacterModel;
		public Vector3 MapCoords;
		public Vector2 ScreenXY;
		public List<Item> equipped, inventory;
		public enum Genders
        {
			male,
			female
        }
		public Genders Gender;
		public Dictionary<string, Item> Inventory;
		public Dictionary<string, ICharacter> Tramily;
		public List<Item> Equipped;
		public Player(IServiceProvider services, GraphicsDeviceManager graphics, string name)
		{
			
			
			Name = name;
			stats = new Stats();
			ScreenXY = new Vector2(0,0);
			CharacterModel = new CharacterModel(services, graphics, ScreenXY, this);
		}

		  public void MoveTo(Location location)
        {
			Location = location;
        }

		
		public State Update(GameTime gameTime) {
			/*ScreenXY.X = trailLocation.Coords.X;
			ScreenXY.Y = trailLocation.Coords.Y;*/
			CharacterModel.ScreenXY = ScreenXY;
			CharacterModel.Update(gameTime);
			return State.MainSettings; }
		public void Draw(SpriteBatch spriteBatch) {

			CharacterModel.Draw(spriteBatch, 1);
		}

	}
}

