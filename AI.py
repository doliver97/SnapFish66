import numpy as np
from keras.models import Sequential
from keras.layers import Dense, Activation

# Converts the input to one hot encoded boolean list
def CharList2Bools(ch):
    boolList = []
    #First character (next player)
    if ch[0] == 'A':
        boolList.extend([True,False])
    else:
        boolList.extend([False,True])
    #Second: trump
    if ch[1] == 'M':
        boolList.extend([True,False,False,False])
    elif ch[1] == 'P':
        boolList.extend([False,True,False,False])
    elif ch[1] == 'T':
        boolList.extend([False,False,True,False])
    else:
        boolList.extend([False,False,False,True])
    # 20 cards
    for i in range(2,22):
        if ch[i] == 'A':
            boolList.extend([True,False,False,False,False,False,False,False])
        elif ch[i] == 'B':
            boolList.extend([False,True,False,False,False,False,False,False])
        elif ch[i] == 'D':
            boolList.extend([False,False,True,False,False,False,False,False])
        elif ch[i] == 'E':
            boolList.extend([False,False,False,True,False,False,False,False])
        elif ch[i] == 'F':
            boolList.extend([False,False,False,False,True,False,False,False])
        elif ch[i] == 'G':
            boolList.extend([False,False,False,False,False,True,False,False])
        elif ch[i] == 'H':
            boolList.extend([False,False,False,False,False,False,True,False])
        elif ch[i] == 'U':
            boolList.extend([False,False,False,False,False,False,False,True])
    #2040
    for i in range(22,26):
        if ch[i] == 'A':
            boolList.extend([True,False,False])
        elif ch[i] == 'B':
            boolList.extend([False,True,False])
        elif ch[i] == 'X':
            boolList.extend([False,False,True])
    return boolList
 

# Convert csv to lists
x_train = []
y_train = []
x_test = []
y_test = []

with open("TrainingData.csv") as csvData:
    for line in csvData:
        splitted = line.strip().split(",")
        x_train.append(CharList2Bools(splitted[0:26]))
        y_train.append(splitted[26:31])

# Put 20% of training to test
p20 = int(len(x_train) * 0.2)
for i in range(p20):
    x_test.append(x_train[i])
    y_test.append(y_train[i])
del x_train[0:p20]
del y_train[0:p20]

# Convert lists to numpy arrays
x_train = np.asarray(x_train, dtype=np.bool)
y_train = np.asarray(y_train, dtype=np.float32)
x_test = np.asarray(x_test, dtype=np.bool)
y_test = np.asarray(y_test, dtype=np.float32)

pass

# Keras starts here

# Init  
model = Sequential()
model.add(Dense(32, input_dim=len(x_train[0])))
model.add(Dense(32))
#model.add(Dense(32))
#model.add(Dense(32))
model.add(Dense(5))
model.add(Activation('relu'))
model.compile(optimizer='rmsprop',loss='mae')

# Train
model.fit(x_train, y_train,epochs=100,batch_size=32, verbose=2)

# Evaluate
score = model.evaluate(x_test, y_test, batch_size=32, verbose=1)
print('Score:' + str(score))

# Make a prediction for d3 test
x = []
x.append(CharList2Bools(['A','P','A','U','A','U','U','D','U','U','H','G','H','G','U','U','A','A','A','G','U','G','X','X','X','X']))
x = np.asarray(x)
y = model.predict(x,batch_size=1,verbose=1)
print(str(y))