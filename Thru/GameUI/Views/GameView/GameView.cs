using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Thru
{
	public class GameView : IView
	{


		public Location currentLocation, trailLocation;
		public MapGameView mapView;
		public GameViewStateMachine stateMachine;
		public GameTime gameTime;
		public PlayGameView playView;
		public Character Player;
		public InventoryGameView inventoryView;
		public GameView(int clientWidth, int clientHeight, IServiceProvider services, GraphicsDeviceManager graphics, GlobalState globalState)
		{


			Player = globalState.Player;
			mapView = new MapGameView(services, clientWidth, clientHeight, graphics, Player, globalState);
			inventoryView = new InventoryGameView(services, graphics, Player, globalState);
			currentLocation = mapView.currentLocation;
			trailLocation = mapView.currentTrailLocation;
			
			playView = new PlayGameView(services, graphics, currentLocation, trailLocation, Player, globalState);
			stateMachine = new GameViewStateMachine(services, graphics, mapView, playView, inventoryView);
			stateMachine.currentState = GameState.Play;
		}

	

		public State Update(GameTime gameTime)
		{

			currentLocation = mapView.currentLocation;
			playView.currentLocation = currentLocation;
			trailLocation = mapView.currentTrailLocation;
			Player.Location = currentLocation;
			Player.TrailLocation = trailLocation;
			stateMachine.Update(gameTime);


			return State.Game;
		}




		public void Draw(GraphicsDeviceManager graphics)
		{

			
			stateMachine.Draw(gameTime);


		}



	}

}