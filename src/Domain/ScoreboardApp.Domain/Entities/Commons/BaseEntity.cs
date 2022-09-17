using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Domain.Entities.Commons
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
    }
}
