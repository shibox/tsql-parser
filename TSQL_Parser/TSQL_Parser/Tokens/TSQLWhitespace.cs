using System;

namespace TSQL.Tokens
{
	public class TSQLWhitespace : TSQLToken
	{
		internal TSQLWhitespace(
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
				return TSQLTokenType.Whitespace;
			}
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
