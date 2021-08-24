﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSQL.Expressions
{
	/// <summary>
	///		Represents uses of * to reference multiple columns within a SQL expression.
	/// </summary>
	public class TSQLMulticolumnExpression : TSQLExpression
	{
		public override TSQLExpressionType Type
		{
			get
			{
				return TSQLExpressionType.Multicolumn;
			}
		}

		public string TableAlias { get; internal set; }
	}
}
