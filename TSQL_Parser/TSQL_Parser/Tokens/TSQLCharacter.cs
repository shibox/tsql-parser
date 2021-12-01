using System;

namespace TSQL.Tokens
{
	public class TSQLCharacter : TSQLToken
	{
		internal TSQLCharacter(
			int beginPosition,
			string text) :
			base(
				beginPosition,
				text)
		{
			Character = TSQLCharacters.Parse(text);
		}



		public override TSQLTokenType Type
		{
			get
			{
				return TSQLTokenType.Character;
			}
		}



		public TSQLCharacters Character
		{
			get;
			private set;
		}

		public override bool IsComplete
		{
			get
			{
				return true;
			}
		}
	}
}
