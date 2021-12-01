using System;

namespace TSQL.Tokens
{
	public class TSQLVariable : TSQLToken
	{
		internal TSQLVariable(
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
				return TSQLTokenType.Variable;
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
