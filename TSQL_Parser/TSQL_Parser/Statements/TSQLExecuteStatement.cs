using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TSQL.Clauses;

namespace TSQL.Statements
{
	public class TSQLExecuteStatement : TSQLStatement
	{
		internal TSQLExecuteStatement()
		{

		}



		public override TSQLStatementType Type
		{
			get
			{
				return TSQLStatementType.Delete;
			}
		}


	}
}
