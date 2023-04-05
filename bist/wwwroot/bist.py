import numpy as np
from sklearn.ensemble import RandomForestRegressor


def predict_price(hacimler, onceki_kapanis_fiyatlari, en_yuksek_fiyatlar, en_dusuk_fiyatlar):
    # Verileri numpy dizilerine d�n��t�rme
    X = np.column_stack((hacimler, onceki_kapanis_fiyatlari, en_yuksek_fiyatlar, en_dusuk_fiyatlar))

    # E�itim verilerini y�kleme
    X_train = np.load('X_train.npy')
    y_train = np.load('y_train.npy')

    # Random Forest Regressor modelini olu�turma ve e�itme
    model = RandomForestRegressor(n_estimators=100, random_state=42)
    model.fit(X_train, y_train)

    # Tahmin yapma
    y_pred = model.predict(X)

    # Tahmin sonucunu d�nd�rme
    return y_pred[0]
