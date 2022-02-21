using Grpc.Core;
using GrpcService;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GrpcService.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override Task<DataRowReply> GetRow(RowIdRequest request, ServerCallContext context)
        {
            var DataTableExample = new DataTable("Sample Table");
            DataTableExample.Columns.Add(new DataColumn("Column 1", typeof(int)));
            DataTableExample.Columns.Add(new DataColumn("Column 2", typeof(string)));
            DataTableExample.Columns.Add(new DataColumn("Column 3", typeof(DateTime)));
            DataTableExample.Rows.Add(new object[] { 1, "Row 1 Name", DateTime.Now });
            DataTableExample.Rows.Add(new object[] { 2, "Row 2 Name", DateTime.Now });
            DataTableExample.Rows.Add(new object[] { 3, "Row 3 Name", DateTime.Now });

            DataRowReply? result = new DataRowReply();

            //Table Headers (Column names)
            var sb = new StringBuilder();
            foreach (DataColumn dc in DataTableExample.Columns) 
            {
                sb.Append(dc.ColumnName);
                sb.Append('\t');
                sb.Append('\t');
            }

            result.Value.Add(sb.ToString().TrimEnd());

            //Table Content (Rows)
            foreach (DataRow datarow in DataTableExample.Rows)
            {
                sb = new StringBuilder();

                for (int col = 0; col < DataTableExample.Columns.Count; col++)
                {
                    sb.Append(datarow[col].ToString());
                    if (col != DataTableExample.Columns.Count)
                    {
                        sb.Append('\t');
                        sb.Append('\t');
                    }
                }

                result.Value.Add(sb.ToString());
            }

            return Task.FromResult(result);
        }
    }
}