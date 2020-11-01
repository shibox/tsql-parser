﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TSQL.Tokens;

namespace TSQL.Expressions.Parsers
{
	internal class TSQLOperatorExpressionParser
	{
		public TSQLOperatorExpression Parse(
			ITSQLTokenizer tokenizer,
			TSQLExpression leftSide)
		{
			TSQLOperatorExpression opExpression = new TSQLOperatorExpression();

			opExpression.LeftSide = leftSide;
			opExpression.Operator = tokenizer.Current.AsOperator;

			opExpression.Tokens.AddRange(leftSide.Tokens);
			opExpression.Tokens.Add(tokenizer.Current);

			while (
				tokenizer.MoveNext() &&
				(
					tokenizer.Current.IsWhitespace() ||
					tokenizer.Current.IsComment()
				))
			{
				opExpression.Tokens.Add(tokenizer.Current);
			}

			TSQLExpression rightSide = new TSQLExpressionFactory().Parse(
				tokenizer);

			// a + (b + (c + d))

			if (
				tokenizer.Current != null &&
				tokenizer.Current.Type.In(
					TSQLTokenType.Operator))
			{
				rightSide = Parse(
					tokenizer,
					rightSide);
			}

			opExpression.RightSide = rightSide;
			opExpression.Tokens.AddRange(rightSide.Tokens);

			return opExpression;
		}
	}
}
