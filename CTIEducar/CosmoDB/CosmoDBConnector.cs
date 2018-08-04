using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace CTIEducar.CosmoDB
{
	public class CosmoDBConnector
	{
		private const string EndpointUrl = "https://ajn.table.cosmosdb.azure.com:443/";
		private const string PrimaryKey = "TLlNmk8dkjScFoAhimKoKDDnmpDoewaBLsvGQprBQvThOhJofdBKvqycHCG9BQGtynicusHbyLdIwCL8GGHTag==";
		private DocumentClient client;

		public async Task DataBaseConnector()
		{
			this.client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
		}
	}
}