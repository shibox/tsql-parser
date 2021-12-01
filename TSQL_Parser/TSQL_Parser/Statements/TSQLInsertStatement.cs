using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TSQL.Clauses;
using TSQL.Expressions;

namespace TSQL.Statements
{
	public class TSQLInsertStatement : TSQLStatement
	{
		internal TSQLInsertStatement()
		{

		}



		public override TSQLStatementType Type
		{
			get
			{
				return TSQLStatementType.Insert;
			}
		}



		public TSQLWithClause With { get; internal set; }

		public TSQLInsertClause Insert { get; internal set; }

		public TSQLOutputClause Output { get; internal set; }

		public TSQLValuesExpression Values { get; internal set; }

		public TSQLSelectStatement Select { get; internal set; }

		public TSQLExecuteStatement Execute { get; internal set; }

		public TSQLDefaultValuesExpression Default { get; internal set; }
	}
}
