﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TSQL.Statements;
using TSQL.Statements.Parsers;
using TSQL.Tokens;
using TSQL.Tokens.Parsers;

namespace TSQL.Expressions.Parsers
{
	internal class TSQLExpressionFactory
	{
		public TSQLExpression Parse(ITSQLTokenizer tokenizer)
		{
			TSQLExpression expression = ParseNext(tokenizer);

			if (
				tokenizer.Current != null &&
				tokenizer.Current.Type.In(
					TSQLTokenType.Operator) &&

				// check for operator =, when expression type is column, and don't parse operator if found
				// e.g. IsFinishedGoods = p.FinishedGoodsFlag

				(
					tokenizer.Current.Text != "=" ||
					expression.Type != TSQLExpressionType.Column
				))
			{
				return new TSQLOperatorExpressionParser().Parse(
					tokenizer,
					expression);
			}
			else
			{
				return expression;
			}
		}

		private static TSQLExpression ParseNext(
			ITSQLTokenizer tokenizer)
		{
			if (tokenizer.Current == null)
			{
				return null;
			}

			// look at the current/first token to determine what to do

			if (tokenizer.Current.Text == "*")
			{
				TSQLMulticolumnExpression simpleMulti = new TSQLMulticolumnExpression();

				simpleMulti.Tokens.Add(tokenizer.Current);

				ReadThroughAnyCommentsOrWhitespace(
					tokenizer,
					simpleMulti.Tokens);

				return simpleMulti;

				// still need to seperately check for p.* below
			}
			// this checks for unary operators, e.g. +, -, and ~
			else if (tokenizer.Current.Type.In(
				TSQLTokenType.Operator))
			{
				return null;
			}
			else if (tokenizer.Current.IsCharacter(
				TSQLCharacters.OpenParentheses))
			{
				List<TSQLToken> tokens = new List<TSQLToken>();

				tokens.Add(tokenizer.Current);

				// read through any whitespace so we can check specifically for a SELECT
				ReadThroughAnyCommentsOrWhitespace(
					tokenizer,
					tokens);

				if (tokenizer.Current.IsKeyword(TSQLKeywords.SELECT))
				{
					#region parse subquery

					TSQLSubqueryExpression subquery = new TSQLSubqueryExpression();

					subquery.Tokens.AddRange(tokens);

					TSQLSelectStatement select = new TSQLSelectStatementParser(tokenizer).Parse();

					subquery.Select = select;

					subquery.Tokens.AddRange(select.Tokens);

					if (tokenizer.Current.IsCharacter(TSQLCharacters.CloseParentheses))
					{
						subquery.Tokens.Add(tokenizer.Current);

						tokenizer.MoveNext();
					}

					return subquery;

					#endregion
				}
				else
				{
					#region parse expression contained/grouped inside parenthesis

					TSQLGroupedExpression group = new TSQLGroupedExpression();

					group.Tokens.AddRange(tokens);

					group.InnerExpression = 
						new TSQLExpressionFactory().Parse(
							tokenizer);
					group.Tokens.AddRange(group.InnerExpression.Tokens);

					if (tokenizer.Current.IsCharacter(
						TSQLCharacters.CloseParentheses))
					{
						group.Tokens.Add(tokenizer.Current);
						tokenizer.MoveNext();
					}

					return group;

					#endregion
				}
			}
			else if (tokenizer.Current.Type.In(
				TSQLTokenType.Variable,
				TSQLTokenType.SystemVariable))
			{
				TSQLVariableExpression variable = new TSQLVariableExpression();
				variable.Tokens.Add(tokenizer.Current);
				variable.Variable = tokenizer.Current.AsVariable;

				ReadThroughAnyCommentsOrWhitespace(
					tokenizer,
					variable.Tokens);

				return variable;
			}
			else if (tokenizer.Current.Type.In(
				TSQLTokenType.BinaryLiteral,
				TSQLTokenType.MoneyLiteral,
				TSQLTokenType.NumericLiteral,
				TSQLTokenType.StringLiteral,
				TSQLTokenType.IncompleteString))
			{
				TSQLConstantExpression constant = new TSQLConstantExpression();

				constant.Literal = tokenizer.Current.AsLiteral;

				constant.Tokens.Add(tokenizer.Current);

				ReadThroughAnyCommentsOrWhitespace(
					tokenizer,
					constant.Tokens);

				return constant;
			}
			else if (tokenizer.Current.IsKeyword(TSQLKeywords.CASE))
			{
				return new TSQLCaseExpressionParser().Parse(tokenizer);
			}
			else if (tokenizer.Current.Type.In(
				TSQLTokenType.SystemColumnIdentifier,
				TSQLTokenType.IncompleteIdentifier))
			{
				TSQLColumnExpression column = new TSQLColumnExpression();

				column.Column = tokenizer.Current.AsSystemColumnIdentifier;

				column.Tokens.Add(tokenizer.Current);

				ReadThroughAnyCommentsOrWhitespace(
					tokenizer,
					column.Tokens);

				return column;
			}
			else if (tokenizer.Current.Type.In(
				TSQLTokenType.SystemIdentifier))
			{
				#region parse system function

				TSQLFunctionExpression function = new TSQLFunctionExpression();

				function.Name = tokenizer.Current.Text;

				function.Tokens.Add(tokenizer.Current);

				if (tokenizer.MoveNext() &&
					tokenizer.Current.IsCharacter(TSQLCharacters.OpenParentheses))
				{
					function.Tokens.Add(tokenizer.Current);

					tokenizer.MoveNext();

					TSQLArgumentList arguments = new TSQLArgumentListParser().Parse(
						tokenizer);

					function.Tokens.AddRange(arguments.Tokens);

					function.Arguments = arguments;

					if (tokenizer.Current.IsCharacter(TSQLCharacters.CloseParentheses))
					{
						function.Tokens.Add(tokenizer.Current);
					}

					ReadThroughAnyCommentsOrWhitespace(
						tokenizer,
						function.Tokens);
				}

				return function;

				#endregion
			}
			else if (tokenizer.Current.Type.In(
				TSQLTokenType.Identifier))
			{
				// multi column with alias

				// or column, with or without alias, or with full explicit table name with up to 5 parts

				// or function, up to 4 part naming

				// find last token up to and including possible first paren
				// if *, then multi column
				// if paren, then function
				// else column

				// alias would be any tokens prior to last period, removing whitespace

				List<TSQLToken> tokens = new List<TSQLToken>();

				tokens.Add(tokenizer.Current);

				while (tokenizer.MoveNext())
				{
					if (tokenizer.Current.IsCharacter(TSQLCharacters.OpenParentheses))
					{
						#region parse function

						tokens.Add(tokenizer.Current);

						tokenizer.MoveNext();

						TSQLArgumentList arguments = new TSQLArgumentListParser().Parse(
							tokenizer);

						TSQLFunctionExpression function = new TSQLFunctionExpression();

						function.Tokens.AddRange(tokens);
						function.Tokens.AddRange(arguments.Tokens);

						function.Arguments = arguments;

						if (tokenizer.Current.IsCharacter(TSQLCharacters.CloseParentheses))
						{
							function.Tokens.Add(tokenizer.Current);
						}

						// TODO: should we keep these as tokens?
						function.Name =
							tokens
								.Where(t => !t.IsComment() && !t.IsWhitespace())
								.Reverse()
								// last token will be the open paren
								.Skip(1)
								.First()
								.AsIdentifier
								.Name;

						tokenizer.MoveNext();

						TSQLTokenParserHelper.ReadCommentsAndWhitespace(
							tokenizer,
							function);

						// look for windowed aggregate
						if (tokenizer.Current.IsKeyword(TSQLKeywords.OVER))
						{
							function.Tokens.Add(tokenizer.Current);

							tokenizer.MoveNext();

							TSQLTokenParserHelper.ReadCommentsAndWhitespace(
								tokenizer,
								function);

							if (tokenizer.Current.IsCharacter(TSQLCharacters.OpenParentheses))
							{
								function.Tokens.Add(tokenizer.Current);

								// recursively look for final close parens
								TSQLTokenParserHelper.ReadUntilStop(
									tokenizer,
									function,
									new List<TSQLFutureKeywords> { },
									new List<TSQLKeywords> { },
									lookForStatementStarts: false);

								if (tokenizer.Current != null &&
									tokenizer.Current.IsCharacter(TSQLCharacters.CloseParentheses))
								{
									function.Tokens.Add(tokenizer.Current);

									tokenizer.MoveNext();
								}
							}
						}

						return function;

						#endregion
					}
					else if (tokenizer.Current.Text == "*")
					{
						#region parse multi column reference

						// e.g. p.*

						TSQLMulticolumnExpression multi = new TSQLMulticolumnExpression();

						multi.Tokens.AddRange(tokens);

						multi.Tokens.Add(tokenizer.Current);

						List<TSQLToken> columnReference = tokens
							.Where(t => !t.IsComment() && !t.IsWhitespace())
							.ToList();

						if (columnReference.Count > 0)
						{
							// p.* will have the single token p in the final list

							// AdventureWorks..ErrorLog.* will have 4 tokens in the final list
							// e.g. {AdventureWorks, ., ., ErrorLog}

							multi.TableReference = columnReference
								.GetRange(0, columnReference
									.FindLastIndex(t => t.IsCharacter(TSQLCharacters.Period)))
								.ToList();
						}

						ReadThroughAnyCommentsOrWhitespace(
							tokenizer,
							multi.Tokens);

						return multi;

						#endregion
					}
					else if (
						tokenizer.Current.IsCharacter(TSQLCharacters.Comma) ||
						tokenizer.Current.IsCharacter(TSQLCharacters.CloseParentheses) ||
						tokenizer.Current.Type.In(
							TSQLTokenType.Keyword, 
							TSQLTokenType.Operator) ||

						// this will be a nasty check, but I don't want to copy the internal logic elsewhere

						// two identifiers in a row means that the second one is an alias
						(
							tokenizer.Current.Type.In(
								TSQLTokenType.Identifier, 
								TSQLTokenType.IncompleteIdentifier) &&
							tokens
								.Where(t => !t.IsComment() && !t.IsWhitespace())
								.LastOrDefault()
								?.Type.In(
									TSQLTokenType.Identifier,
									TSQLTokenType.BinaryLiteral,
									TSQLTokenType.MoneyLiteral,
									TSQLTokenType.NumericLiteral,
									TSQLTokenType.StringLiteral,
									TSQLTokenType.SystemColumnIdentifier,
									TSQLTokenType.SystemIdentifier,
									TSQLTokenType.SystemVariable,
									TSQLTokenType.Variable
									) == true // Operator '&&' cannot be applied to operands of type 'bool' and 'bool?'
						))
					{
						TSQLColumnExpression column = new TSQLColumnExpression();

						column.Tokens.AddRange(tokens);

						List<TSQLToken> columnReference = tokens
							.Where(t => !t.IsComment() && !t.IsWhitespace())
							.ToList();

						if (columnReference.Count > 1)
						{
							// p.ProductID will have the single token p in the final list

							// AdventureWorks..ErrorLog.ErrorLogID will have 4 tokens in the final list
							// e.g. {AdventureWorks, ., ., ErrorLog}

							column.TableReference = columnReference
								.GetRange(0, columnReference
									.FindLastIndex(t => t.IsCharacter(TSQLCharacters.Period)))
								.ToList();
						}

						column.Column = columnReference
							.Last()
							.AsIdentifier;

						return column;
					}
					else
					{
						tokens.Add(tokenizer.Current);
					}
				}

				return null;
			}
			else
			{
				return null;
			}
		}

		private static void ReadThroughAnyCommentsOrWhitespace(
			ITSQLTokenizer tokenizer,
			List<TSQLToken> savedTokens)
		{
			while (
				tokenizer.MoveNext() &&
				(
					tokenizer.Current.IsWhitespace() ||
					tokenizer.Current.IsComment())
				)
			{
				savedTokens.Add(tokenizer.Current);
			}
		}
	}
}
