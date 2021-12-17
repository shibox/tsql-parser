using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSQL
{
	public struct TSQLVariables
	{
		private static Dictionary<string, TSQLVariables> variableLookup = new(StringComparer.OrdinalIgnoreCase);

		public static readonly TSQLVariables None = new("");

		public static readonly TSQLVariables CONNECTIONS = new("@@CONNECTIONS");
		public static readonly TSQLVariables MAX_CONNECTIONS = new("@@MAX_CONNECTIONS");
		public static readonly TSQLVariables CPU_BUSY = new("@@CPU_BUSY");
		public static readonly TSQLVariables ERROR = new("@@ERROR");
		public static readonly TSQLVariables IDENTITY = new("@@IDENTITY");
		public static readonly TSQLVariables IDLE = new("@@IDLE");
		public static readonly TSQLVariables IO_BUSY = new("@@IO_BUSY");
		public static readonly TSQLVariables LANGID = new("@@LANGID");
		public static readonly TSQLVariables LANGUAGE = new("@@LANGUAGE");
		public static readonly TSQLVariables MAXCHARLEN = new("@@MAXCHARLEN");
		public static readonly TSQLVariables PACK_RECEIVED = new("@@PACK_RECEIVED");
		public static readonly TSQLVariables PACK_SENT = new("@@PACK_SENT");
		public static readonly TSQLVariables PACKET_ERRORS = new("@@PACKET_ERRORS");
		public static readonly TSQLVariables ROWCOUNT = new("@@ROWCOUNT");
		public static readonly TSQLVariables SERVERNAME = new("@@SERVERNAME");
		public static readonly TSQLVariables SPID = new("@@SPID");
		public static readonly TSQLVariables TEXTSIZE = new("@@TEXTSIZE");
		public static readonly TSQLVariables TIMETICKS = new("@@TIMETICKS");
		public static readonly TSQLVariables TOTAL_ERRORS = new("@@TOTAL_ERRORS");
		public static readonly TSQLVariables TOTAL_READ = new("@@TOTAL_READ");
		public static readonly TSQLVariables TOTAL_WRITE = new("@@TOTAL_WRITE");
		public static readonly TSQLVariables TRANCOUNT = new("@@TRANCOUNT");
		public static readonly TSQLVariables VERSION = new("@@VERSION");

		private readonly string Variable;

		private TSQLVariables(string variable)
		{
			Variable = variable;
			if (variable.Length > 0)
			{
				variableLookup[variable] = this;
			}
		}

		public static TSQLVariables Parse(string token)
		{
			if (
				!string.IsNullOrEmpty(token) &&
				variableLookup.ContainsKey(token))
			{
				return variableLookup[token];
			}
			else
			{
				return None;
			}
		}

		public static bool IsVariable(string token)
		{
			if (!string.IsNullOrWhiteSpace(token))
			{
				return variableLookup.ContainsKey(token);
			}
			else
			{
				return false;
			}
		}

		public bool In(params TSQLVariables[] variables)
		{
			return
				variables != null &&
				variables.Contains(this);
		}



		public static bool operator ==(
			TSQLVariables a,
			TSQLVariables b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(
			TSQLVariables a,
			TSQLVariables b)
		{
			return !(a == b);
		}

		public bool Equals(TSQLVariables obj)
		{
			return Variable == obj.Variable;
		}

		public override bool Equals(object obj)
		{
			if (obj is TSQLVariables)
			{
				return Equals((TSQLVariables)obj);
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return Variable.GetHashCode();
		}


	}
}
