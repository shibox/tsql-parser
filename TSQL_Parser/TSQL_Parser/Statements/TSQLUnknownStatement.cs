using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSQL.Statements
{
	public class TSQLUnknownStatement : TSQLStatement
	{
		internal TSQLUnknownStatement()
		{

		}



		public override TSQLStatementType Type
		{
			get
			{
				return TSQLStatementType.Unknown;
			}
		}


	}
}
