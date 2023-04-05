import numpy as np
from sklearn.ensemble import RandomForestRegressor


def predict_price(hacimler, onceki_kapanis_fiyatlari, en_yuksek_fiyatlar, en_dusuk_fiyatlar):
    # Verileri numpy dizilerine dönüþtürme
    X = np.column_stack((hacimler, onceki_kapanis_fiyatlari, en_yuksek_fiyatlar, en_dusuk_fiyatlar))

    # Eðitim verilerini yükleme
    X_train = np.load('X_train.npy')
    y_train = np.load('y_train.npy')

    # Random Forest Regressor modelini oluþturma ve eðitme
    model = RandomForestRegressor(n_estimators=100, random_state=42)
    model.fit(X_train, y_train)

    # Tahmin yapma
    y_pred = model.predict(X)

    # Tahmin sonucunu döndürme
    return y_pred[0]
