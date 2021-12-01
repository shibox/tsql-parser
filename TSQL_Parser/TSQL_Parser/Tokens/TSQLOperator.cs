using System;

namespace TSQL.Tokens
{
	public class TSQLOperator : TSQLToken
	{
		internal TSQLOperator(
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
				return TSQLTokenType.Operator;
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
