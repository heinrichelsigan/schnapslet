using System;

namespace Schnapsen {

	/// <summary>
	/// Schnapslate template class for schnapsen
	/// </summary>
	public class Schnapslate {
		/// <summary>
		/// Color of playingcard:
		/// Hearts=Herz, Clubs=Treff, Diamonds=Karo, Spades=Pik 
		/// </summary>
		public enum Color { NoColor=0, Hearts=1, Clubs=2, Diamonds=3, Spades=4 };
		/// <summary>
		/// Value of playingcard: 
		/// Jack=2, Queen=3, King=4, Ten=10, Ace=11
		/// </summary>
		public enum CardValue { NoValue=0, Jack=2, Queen=3, King=4, Ten=10, Ace=11 };
		
		public Schnapslate() {
		}
	}
}
