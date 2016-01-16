using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomer.Models;

namespace Gomer.Events
{
    public class ModelEventArgs<TModel>
    {
        public TModel Model { get; set; }
    }
}
