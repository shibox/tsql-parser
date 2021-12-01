using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSQL.Tokens
{
	public class TSQLSystemVariable : TSQLVariable
	{
		internal TSQLSystemVariable(
			int beginPosition,
			string text) :
			base(
				beginPosition,
				text)
		{
			Variable = TSQLVariables.Parse(text);
		}



		public override TSQLTokenType Type
		{
			get
			{
				return TSQLTokenType.SystemVariable;
			}
		}



		public TSQLVariables Variable
		{
			get;
			private set;
		}
	}
}
