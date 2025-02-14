﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TSQL.Clauses;

namespace TSQL.Statements
{
	public class TSQLUpdateStatement : TSQLStatement
	{
		internal TSQLUpdateStatement()
		{

		}



		public override TSQLStatementType Type
		{
			get
			{
				return TSQLStatementType.Update;
			}
		}



		public TSQLWithClause With { get; internal set; }

		public TSQLUpdateClause Update { get; internal set; }

		public TSQLOutputClause Output { get; internal set; }

		public TSQLSetClause Set { get; internal set; }

		public TSQLFromClause From { get; internal set; }

		public TSQLWhereClause Where { get; internal set; }

		public TSQLOptionClause Option { get; internal set; }
	}
}
