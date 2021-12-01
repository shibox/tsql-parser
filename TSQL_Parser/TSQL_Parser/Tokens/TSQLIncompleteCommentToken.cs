using System;

namespace TSQL.Tokens
{
	public class TSQLIncompleteCommentToken : TSQLIncompleteToken
	{
		internal TSQLIncompleteCommentToken(
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
				return TSQLTokenType.IncompleteComment;
			}
		}


	}
}
