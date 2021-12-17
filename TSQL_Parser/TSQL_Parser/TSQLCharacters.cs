using System;
using System.Collections.Generic;
using System.Linq;

namespace TSQL
{
	public struct TSQLCharacters
	{
		private static Dictionary<string, TSQLCharacters> characterLookup = new(StringComparer.OrdinalIgnoreCase);

		public static readonly TSQLCharacters None = new TSQLCharacters(string.Empty);

		public static readonly TSQLCharacters Comma = new(",");
		public static readonly TSQLCharacters Semicolon = new(";");
		public static readonly TSQLCharacters OpenParentheses = new("(");
		public static readonly TSQLCharacters CloseParentheses = new(")");
		public static readonly TSQLCharacters Period = new(".");

		private readonly string Token;

		private TSQLCharacters(
			string token)
		{
			Token = token;
			if (token.Length > 0)
			{
				characterLookup[token] = this;
			}
		}

		public static TSQLCharacters Parse(
			string token)
		{
			if (
				!string.IsNullOrEmpty(token) &&
				characterLookup.ContainsKey(token))
			{
				return characterLookup[token];
			}
			else
			{
				return None;
			}
		}

		public static bool IsCharacter(
			string token)
		{
			if (!string.IsNullOrWhiteSpace(token))
			{
				return characterLookup.ContainsKey(token);
			}
			else
			{
				return false;
			}
		}

		public bool In(params TSQLCharacters[] characters)
		{
			return
				characters != null &&
				characters.Contains(this);
		}



		public static bool operator ==(
			TSQLCharacters a,
			TSQLCharacters b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(
			TSQLCharacters a,
			TSQLCharacters b)
		{
			return !(a == b);
		}

		public bool Equals(TSQLCharacters obj)
		{
			return Token == obj.Token;
		}

		public override bool Equals(object obj)
		{
			if (obj is TSQLCharacters)
			{
				return Equals((TSQLCharacters)obj);
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return Token.GetHashCode();
		}


	}

}
