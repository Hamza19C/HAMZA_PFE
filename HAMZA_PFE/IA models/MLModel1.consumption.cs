﻿// This file was auto-generated by ML.NET Model Builder.
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
namespace HAMZA_PFE
{
    public partial class MLModel1
    {
        /// <summary>
        /// model input class for MLModel1.
        /// </summary>
        #region model input class
        public class ModelInput
        {
            [LoadColumn(0)]
            [ColumnName(@"hash")]
            public string Hash { get; set; }

            [LoadColumn(1)]
            [ColumnName(@"millisecond")]
            public float Millisecond { get; set; }

            [LoadColumn(2)]
            [ColumnName(@"classification")]
            public string Classification { get; set; }

            [LoadColumn(3)]
            [ColumnName(@"state")]
            public float State { get; set; }

            [LoadColumn(4)]
            [ColumnName(@"usage_counter")]
            public float Usage_counter { get; set; }

            [LoadColumn(5)]
            [ColumnName(@"prio")]
            public float Prio { get; set; }

            [LoadColumn(6)]
            [ColumnName(@"static_prio")]
            public float Static_prio { get; set; }

            [LoadColumn(7)]
            [ColumnName(@"normal_prio")]
            public float Normal_prio { get; set; }

            [LoadColumn(8)]
            [ColumnName(@"policy")]
            public float Policy { get; set; }

            [LoadColumn(9)]
            [ColumnName(@"vm_pgoff")]
            public float Vm_pgoff { get; set; }

            [LoadColumn(10)]
            [ColumnName(@"vm_truncate_count")]
            public float Vm_truncate_count { get; set; }

            [LoadColumn(11)]
            [ColumnName(@"task_size")]
            public float Task_size { get; set; }

            [LoadColumn(12)]
            [ColumnName(@"cached_hole_size")]
            public float Cached_hole_size { get; set; }

            [LoadColumn(13)]
            [ColumnName(@"free_area_cache")]
            public float Free_area_cache { get; set; }

            [LoadColumn(14)]
            [ColumnName(@"mm_users")]
            public float Mm_users { get; set; }

            [LoadColumn(15)]
            [ColumnName(@"map_count")]
            public float Map_count { get; set; }

            [LoadColumn(16)]
            [ColumnName(@"hiwater_rss")]
            public float Hiwater_rss { get; set; }

            [LoadColumn(17)]
            [ColumnName(@"total_vm")]
            public float Total_vm { get; set; }

            [LoadColumn(18)]
            [ColumnName(@"shared_vm")]
            public float Shared_vm { get; set; }

            [LoadColumn(19)]
            [ColumnName(@"exec_vm")]
            public float Exec_vm { get; set; }

            [LoadColumn(20)]
            [ColumnName(@"reserved_vm")]
            public float Reserved_vm { get; set; }

            [LoadColumn(21)]
            [ColumnName(@"nr_ptes")]
            public float Nr_ptes { get; set; }

            [LoadColumn(22)]
            [ColumnName(@"end_data")]
            public float End_data { get; set; }

            [LoadColumn(23)]
            [ColumnName(@"last_interval")]
            public float Last_interval { get; set; }

            [LoadColumn(24)]
            [ColumnName(@"nvcsw")]
            public float Nvcsw { get; set; }

            [LoadColumn(25)]
            [ColumnName(@"nivcsw")]
            public float Nivcsw { get; set; }

            [LoadColumn(26)]
            [ColumnName(@"min_flt")]
            public float Min_flt { get; set; }

            [LoadColumn(27)]
            [ColumnName(@"maj_flt")]
            public float Maj_flt { get; set; }

            [LoadColumn(28)]
            [ColumnName(@"fs_excl_counter")]
            public float Fs_excl_counter { get; set; }

            [LoadColumn(29)]
            [ColumnName(@"lock")]
            public float Lock { get; set; }

            [LoadColumn(30)]
            [ColumnName(@"utime")]
            public float Utime { get; set; }

            [LoadColumn(31)]
            [ColumnName(@"stime")]
            public float Stime { get; set; }

            [LoadColumn(32)]
            [ColumnName(@"gtime")]
            public float Gtime { get; set; }

            [LoadColumn(33)]
            [ColumnName(@"cgtime")]
            public float Cgtime { get; set; }

            [LoadColumn(34)]
            [ColumnName(@"signal_nvcsw")]
            public float Signal_nvcsw { get; set; }

        }

        #endregion

        /// <summary>
        /// model output class for MLModel1.
        /// </summary>
        #region model output class
        public class ModelOutput
        {
            [ColumnName(@"hash")]
            public float[] Hash { get; set; }

            [ColumnName(@"millisecond")]
            public float Millisecond { get; set; }

            [ColumnName(@"classification")]
            public uint Classification { get; set; }

            [ColumnName(@"state")]
            public float State { get; set; }

            [ColumnName(@"usage_counter")]
            public float Usage_counter { get; set; }

            [ColumnName(@"prio")]
            public float Prio { get; set; }

            [ColumnName(@"static_prio")]
            public float Static_prio { get; set; }

            [ColumnName(@"normal_prio")]
            public float Normal_prio { get; set; }

            [ColumnName(@"policy")]
            public float Policy { get; set; }

            [ColumnName(@"vm_pgoff")]
            public float Vm_pgoff { get; set; }

            [ColumnName(@"vm_truncate_count")]
            public float Vm_truncate_count { get; set; }

            [ColumnName(@"task_size")]
            public float Task_size { get; set; }

            [ColumnName(@"cached_hole_size")]
            public float Cached_hole_size { get; set; }

            [ColumnName(@"free_area_cache")]
            public float Free_area_cache { get; set; }

            [ColumnName(@"mm_users")]
            public float Mm_users { get; set; }

            [ColumnName(@"map_count")]
            public float Map_count { get; set; }

            [ColumnName(@"hiwater_rss")]
            public float Hiwater_rss { get; set; }

            [ColumnName(@"total_vm")]
            public float Total_vm { get; set; }

            [ColumnName(@"shared_vm")]
            public float Shared_vm { get; set; }

            [ColumnName(@"exec_vm")]
            public float Exec_vm { get; set; }

            [ColumnName(@"reserved_vm")]
            public float Reserved_vm { get; set; }

            [ColumnName(@"nr_ptes")]
            public float Nr_ptes { get; set; }

            [ColumnName(@"end_data")]
            public float End_data { get; set; }

            [ColumnName(@"last_interval")]
            public float Last_interval { get; set; }

            [ColumnName(@"nvcsw")]
            public float Nvcsw { get; set; }

            [ColumnName(@"nivcsw")]
            public float Nivcsw { get; set; }

            [ColumnName(@"min_flt")]
            public float Min_flt { get; set; }

            [ColumnName(@"maj_flt")]
            public float Maj_flt { get; set; }

            [ColumnName(@"fs_excl_counter")]
            public float Fs_excl_counter { get; set; }

            [ColumnName(@"lock")]
            public float Lock { get; set; }

            [ColumnName(@"utime")]
            public float Utime { get; set; }

            [ColumnName(@"stime")]
            public float Stime { get; set; }

            [ColumnName(@"gtime")]
            public float Gtime { get; set; }

            [ColumnName(@"cgtime")]
            public float Cgtime { get; set; }

            [ColumnName(@"signal_nvcsw")]
            public float Signal_nvcsw { get; set; }

            [ColumnName(@"Features")]
            public float[] Features { get; set; }

            [ColumnName(@"PredictedLabel")]
            public string PredictedLabel { get; set; }

            [ColumnName(@"Score")]
            public float[] Score { get; set; }

        }

        #endregion

        private static string MLNetModelPath = Path.GetFullPath("MLModel1.mlnet");

        public static readonly Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictEngine = new Lazy<PredictionEngine<ModelInput, ModelOutput>>(() => CreatePredictEngine(), true);


        private static PredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
        {
            var mlContext = new MLContext();
            ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out var _);
            return mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
        }

        /// <summary>
        /// Use this method to predict scores for all possible labels.
        /// </summary>
        /// <param name="input">model input.</param>
        /// <returns><seealso cref=" ModelOutput"/></returns>
        public static IOrderedEnumerable<KeyValuePair<string, float>> PredictAllLabels(ModelInput input)
        {
            var predEngine = PredictEngine.Value;
            var result = predEngine.Predict(input);
            return GetSortedScoresWithLabels(result);
        }

        /// <summary>
        /// Map the unlabeled result score array to the predicted label names.
        /// </summary>
        /// <param name="result">Prediction to get the labeled scores from.</param>
        /// <returns>Ordered list of label and score.</returns>
        /// <exception cref="Exception"></exception>
        public static IOrderedEnumerable<KeyValuePair<string, float>> GetSortedScoresWithLabels(ModelOutput result)
        {
            var unlabeledScores = result.Score;
            var labelNames = GetLabels(result);

            Dictionary<string, float> labledScores = new Dictionary<string, float>();
            for (int i = 0; i < labelNames.Count(); i++)
            {
                // Map the names to the predicted result score array
                var labelName = labelNames.ElementAt(i);
                labledScores.Add(labelName.ToString(), unlabeledScores[i]);
            }

            return labledScores.OrderByDescending(c => c.Value);
        }

        /// <summary>
        /// Get the ordered label names.
        /// </summary>
        /// <param name="result">Predicted result to get the labels from.</param>
        /// <returns>List of labels.</returns>
        /// <exception cref="Exception"></exception>
        private static IEnumerable<string> GetLabels(ModelOutput result)
        {
            var schema = PredictEngine.Value.OutputSchema;

            var labelColumn = schema.GetColumnOrNull("classification");
            if (labelColumn == null)
            {
                throw new Exception("classification column not found. Make sure the name searched for matches the name in the schema.");
            }

            // Key values contains an ordered array of the possible labels. This allows us to map the results to the correct label value.
            var keyNames = new VBuffer<ReadOnlyMemory<char>>();
            labelColumn.Value.GetKeyValues(ref keyNames);
            return keyNames.DenseValues().Select(x => x.ToString());
        }

        /// <summary>
        /// Use this method to predict on <see cref="ModelInput"/>.
        /// </summary>
        /// <param name="input">model input.</param>
        /// <returns><seealso cref=" ModelOutput"/></returns>
        public static ModelOutput Predict(ModelInput input)
        {
            var predEngine = PredictEngine.Value;
            return predEngine.Predict(input);
        }
    }
}
