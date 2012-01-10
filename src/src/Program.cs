using System;
using weka.core;
using weka.classifiers;
using System.Diagnostics;

public class Program
{
  public static void Main(string[] args) {
    try {
      int runs = 1;
      string algo = "";
      string data = "";
      if(args.Length>0) runs = Convert.ToInt32(args[0]);
      if(args.Length>1) algo = args[1];
      if(args.Length>2) data = args[2];

      Stopwatch read = new Stopwatch(), 
        build = new Stopwatch(), 
        classify = new Stopwatch();
      for (int cnt=0; cnt<runs; cnt++) {
        read.Start();
        Instances train = new Instances(new java.io.FileReader(data+"train.arff"));
        train.setClassIndex(train.numAttributes() - 1);
        Instances test = new Instances(new java.io.FileReader(data+"test.arff"));
        test.setClassIndex(test.numAttributes() - 1);
        read.Stop();

        Classifier[] clList = {
          new weka.classifiers.bayes.NaiveBayes(),
          new weka.classifiers.trees.RandomForest(),
          new weka.classifiers.trees.J48(),
          new weka.classifiers.functions.MultilayerPerceptron(),
          new weka.classifiers.rules.ConjunctiveRule(),
          new weka.classifiers.functions.SMO()
        };

        build.Start();
        foreach (Classifier classifier in clList) {
          if(algo.Equals("") || classifier.getClass().getSimpleName().Equals(algo))
              classifier.buildClassifier(train);
        }
        build.Stop();

        classify.Start();
        foreach (Classifier classifier in clList) {
          if(algo.Equals("") || classifier.getClass().getSimpleName().Equals(algo)) {
              int numCorrect = 0;
              for (int i = 0; i < test.numInstances(); i++)
              {
                  if (classifier.classifyInstance(test.instance(i)) == test.instance(i).classValue())
                      numCorrect++;
              }
              //Console.Write(classifier.getClass().getSimpleName() + "\t" + numCorrect + " out of " + test.numInstances() + " correct (" +(100.0 * numCorrect / test.numInstances()) + "%)");
          }
        }
        classify.Stop();
      }
      Console.WriteLine("{\""+ algo + "\"," + read.ElapsedMilliseconds + "," + build.ElapsedMilliseconds + "," + classify.ElapsedMilliseconds + "," + (read.ElapsedMilliseconds+build.ElapsedMilliseconds+classify.ElapsedMilliseconds)+"};");
    } catch (java.lang.Exception e){
      e.printStackTrace();
    }
  }
}