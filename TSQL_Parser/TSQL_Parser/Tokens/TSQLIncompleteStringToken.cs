using System;

namespace TSQL.Tokens
{
	public class TSQLIncompleteStringToken : TSQLIncompleteToken
	{
		internal TSQLIncompleteStringToken(
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
				return TSQLTokenType.IncompleteString;
			}
		}


	}
}
