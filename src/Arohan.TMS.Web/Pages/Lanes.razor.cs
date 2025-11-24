using Microsoft.AspNetCore.Components;
using Arohan.TMS.Application.Features.Lanes.DTOs;
using Arohan.TMS.Application.Features.Lanes.Queries.GetLanes;
using Arohan.TMS.Application.Features.Lanes.Commands.CreateLane;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arohan.TMS.Web.Pages
{
    public partial class Lanes
    {
        private bool loading = true;
        private List<LaneDto> lanes = new();
        private string newName = string.Empty;
        private int newPlazaId = 1;

        protected override async Task OnInitializedAsync()
        {
            await LoadLanesAsync();
        }

        private async Task LoadLanesAsync()
        {
            loading = true;
            try
            {
                lanes = await Mediator.Send(new GetLanesQuery());
            }
            finally
            {
                loading = false;
            }
        }

        private async Task CreateLane()
        {
            loading = true;
            try
            {
                var cmd = new CreateLaneCommand(newPlazaId, newName, null, null, "None");
                var created = await Mediator.Send(cmd);

                // refresh
                await LoadLanesAsync();

                newName = string.Empty;
                newPlazaId = 1;
            }
            finally
            {
                loading = false;
            }
        }
    }
}
