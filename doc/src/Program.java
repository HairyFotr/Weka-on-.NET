import weka.core.Instance;
import weka.core.Instances;
import weka.classifiers.*;
import java.io.FileReader;

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
          new weka.classifiers.trees.RandomForest(),
          new weka.classifiers.bayes.NaiveBayes()
        };
                
        long buildStart = System.nanoTime();
        for (Classifier classifier : clList) {
          if(algo.equals("") || classifier.getClass().getSimpleName().equals(algo))
              classifier.buildClassifier(train);
        }
        build += System.nanoTime()-buildStart;

        long classifyStart = System.nanoTime();
        for (Classifier classifier : clList) {
          if(algo.equals("") || classifier.getClass().getSimpleName().equals(algo)) {
              int numCorrect = 0;
              for (int i = 0; i < test.numInstances(); i++)
              {
                  if (classifier.classifyInstance(test.instance(i)) == test.instance(i).classValue())
                      numCorrect++;
              }
              //System.out.print(classifier.getClass().getSimpleName() + "\t");
              //System.out.println(numCorrect + " out of " + test.numInstances() + " correct (" + (100.0 * numCorrect / test.numInstances()) + "%)");
          }
        }
        classify += System.nanoTime()-classifyStart;
      }
      System.out.println("read: " + (read/1000000));
      System.out.println("build: " + (build/1000000));
      System.out.println("classify: " + (classify/1000000));
      System.out.println("total: " + (read+build+classify)/1000000);
    } catch (Exception e) {
      e.printStackTrace();
    }
  }
}