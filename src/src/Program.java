import weka.core.Instance;
import weka.core.Instances;
import weka.classifiers.*;
import java.io.FileReader;
import java.util.Scanner;

class Program {
  public static void main(String args[]) {
    try {
      int runs = 1;
      String algo = "";
      String data = "";
      if(args.length>0) runs = Integer.parseInt(args[0]);
      if(args.length>1) algo = args[1];
      if(args.length>2) data = args[2];
            
      long read=0, build=0, classify=0;
      for (int cnt=0; cnt<runs; cnt++) {
        long readStart = System.nanoTime();
        Instances train = new Instances(new FileReader(data+"train.arff"));
        train.setClassIndex(train.numAttributes() - 1);
        Instances test = new Instances(new FileReader(data+"test.arff"));
        test.setClassIndex(test.numAttributes() - 1);
        read += System.nanoTime()-readStart;
                
        Classifier[] clList = {
          new weka.classifiers.bayes.NaiveBayes(),
          new weka.classifiers.trees.RandomForest(),
          new weka.classifiers.trees.J48(),
          new weka.classifiers.functions.MultilayerPerceptron(),
          new weka.classifiers.rules.ConjunctiveRule(),
          new weka.classifiers.functions.SMO()
        };
                
        long buildStart = System.nanoTime();
        for (Classifier classifier : clList) {
          if(algo.equals("") || algo.equals("All") || classifier.getClass().getSimpleName().equals(algo))
              classifier.buildClassifier(train);
        }
        build += System.nanoTime()-buildStart;

        long classifyStart = System.nanoTime();
        for (Classifier classifier : clList) {
          if(algo.equals("") || algo.equals("All") || classifier.getClass().getSimpleName().equals(algo)) {
              int numCorrect = 0;
              for (int i = 0; i < test.numInstances(); i++)
              {
                  if (classifier.classifyInstance(test.instance(i)) == test.instance(i).classValue())
                      numCorrect++;
              }
              //System.out.print(classifier.getClass().getSimpleName() + "\t" + numCorrect + " out of " + test.numInstances() + " correct (" + (100.0 * numCorrect / test.numInstances()) + "%)");
          }
        }
        classify += System.nanoTime()-classifyStart;
      }
      read /= 1000000;
      build /= 1000000;
      classify /= 1000000;
      System.out.println("{\""+ algo + "\"," + read + "," + build + "," + classify + "," + (read+build+classify)+"};");
      if(args.length>3) {
        (new Scanner(System.in)).nextLine();
      }
    } catch (Exception e) {
      e.printStackTrace();
    }
}
}