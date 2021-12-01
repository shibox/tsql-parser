using System;

namespace TSQL.Tokens
{
	public class TSQLIncompleteIdentifierToken : TSQLIncompleteToken
	{
		internal TSQLIncompleteIdentifierToken(
			int beginPosition,
			string text) :
			base(
				beginPosition,
				text)
		{

		}



		public override TSQLTokenType Type
		{
			get
			{
				return TSQLTokenType.IncompleteIdentifier;
			}
		}


	}
}
