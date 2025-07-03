using Contract.DAOModel;


namespace Contract.Repository
{
    public interface IChartRepository
    {
        public ChartModel GetChartById(string id);
        public List<string> GetBpiIdsByChartId(string chartId);
        public string? GetChartIdByName(string name);

        public string CreateChart(string chartName);
        public void EditChart(ChartModel model);
        public void UpdateChartBpiMaps(string chartId, List<string> bpiIds);
        public List<string> DeleteChartById(string id);

    }
}
