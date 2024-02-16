﻿using ELearn.Domain.Entities;
using ELearn.Domain.Interfaces.Base;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Domain.Interfaces
{
    public interface IAnnouncmentRepo : IBaseRepo<Announcement>
    {        
        //Send Announcement To Multiple Users (Not Deecided)
    }
}