using KubeLife.Domain.Models;
using KubeLife.Kubernetes.Models;
using Microsoft.AspNetCore.Components;
using PSC.Blazor.Components.Chartjs.Models.Bar;
using PSC.Blazor.Components.Chartjs;
using PSC.Blazor.Components.Chartjs.Models.Common;
using KubeLife.Core.Extensions;

namespace KubeLife.BlazorApp.Components
{
    public partial class HistoryChartShow
    {
        //https://github.com/erossini/BlazorChartjs
        [Parameter]
        public KubeCronJobModelView CronJobDetail { get; set; }

        private BarChartConfig _config1;
        private Chart _chart1;
        private const int DisplayCount = 10;

        protected override Task OnInitializedAsync()
        {
            _config1 = new BarChartConfig()
            {
                Options = new Options()
                {
                    Plugins = new Plugins()
                    {
                        Legend = new Legend()
                        {
                            Align = Align.Center,
                            Display = false,
                            Position = LegendPosition.Right
                        }
                    },
                    Scales = new Dictionary<string, Axis>()
                    {
                        {
                            Scales.XAxisId, new Axis()
                            {
                                Stacked = true,
                                Ticks = new Ticks()
                                {
                                    MaxRotation = 0,
                                    MinRotation = 0
                                }
                            }
                        },
                        {
                            Scales.YAxisId, new Axis()
                            {
                                Stacked = true
                            }
                        }
                    }
                }
            };

            var rnd = new Random();
            var val1 = rnd.NextInt64(0, 20);
            var val2 = rnd.NextInt64(0, 20);

            var runsOfCronjob = CronJobDetail.JobDetails.Take(DisplayCount).OrderBy(x => x.StartTime);
            var lbls = new List<string>();
            var vls = new List<decimal?>();
            var clrs = new List<string>();
            foreach (var itm in runsOfCronjob)
            {
                lbls.Add(itm.StartTime.ToDateFormat());
                vls.Add((decimal?)(itm.IsSuccess ? itm.Duration : -1 * itm.Duration));
                clrs.Add((itm.IsSuccess ? "Blue" : "Red"));
            }

            _config1.Data.Labels = lbls;
            _config1.Data.Datasets.Add(new BarDataset()
            {
                Label = "Time(min)",
                Data = vls,
                BackgroundColor = clrs,
                BorderWidth = 1
            });

            return base.OnInitializedAsync();
        }
    }
}
